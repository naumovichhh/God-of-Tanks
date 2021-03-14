using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class Tank : ShellCollider, IObjectToSpawn
{
    public Transform turret;
    [SerializeField] private TankInfo tankInfo;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform physicalTurret;
    [SerializeField] private List<ColliderNumberPair> armorList;
    private Dictionary<Collider, float> armor = new Dictionary<Collider, float>();
    new private Rigidbody rigidbody;
    private TankController tankController;
    private bool fireFlag;
    private Stopwatch rechargeStopwatch;
    private float _health;
    public UnityFloatEvent healthChangedEvent;
    public event Action destroyedEvent;
    public event Action fireEvent;
    public event Action<float> hitEvent;
    public event Action turretStartEvent, turretEndEvent;
    public event Action driveStartEvent, driveEndEvent;
    public event Action bounceEvent;
    private bool turretRotateFlag;
    private bool driveFlag;
    private bool justCollided;

    public float rechargeDurationRead => tankInfo.rechargeDuration;
    public float turretTurnSpeed => tankInfo.turretTurnSpeed;
    public float rechargeProcess
    {
        get;
        private set;
    }
    public float health => _health;
    public float turretRotationAngle
    {
        get
        {
            Vector3 rotationDestination, rotationVector;
            rotationDestination = new Vector3(tankController.aimPosition.x, physicalTurret.position.y, tankController.aimPosition.z);
            rotationVector = (rotationDestination - physicalTurret.position).normalized;
            return Vector3.Angle(rotationVector, -physicalTurret.up);
        }
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        tankController = GetComponent<TankController>();
        foreach (var pair in armorList)
        {
            armor.Add(pair.collider, pair.number);
        }
    }

    private void OnEnable()
    {
        // Reset all
        rigidbody.inertiaTensorRotation = Quaternion.identity;
        rechargeProcess = 0;
        _health = 100;
        rechargeStopwatch = new Stopwatch();
        rechargeStopwatch.Start();
    }

    private void Update()
    {
        if (tankController.fireInput > 0)
        {
            fireFlag = true;
        }
    }

    private void FixedUpdate()
    {
        Move();
        RestrictMaxSpeed();
        RotateTurret();
        Recharge();
        Fire();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (justCollided)
            return;
        
        Tank tank = collision.gameObject.GetComponent<Tank>();
        if (tank != null)
        {
            float velocity = collision.relativeVelocity.magnitude;
            if (velocity > 1.9f)
            {
                GetDamaged(velocity * 2.5f);
            }

            justCollided = true;
            Task.Run(() =>
            {
                Thread.Sleep(100);
                justCollided = false;
            });
        }
    }

    public bool IsOverlapped()
    {
        BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();
        foreach (var collider in colliders)
        {
            if (Physics.OverlapBox(collider.transform.position + collider.center, collider.size * transform.lossyScale.x, transform.rotation).Length > 1)
            {
                return true;
            }
        }

        return false;
    }

    public void Healed(float healthPoints)
    {
        _health += healthPoints;
        if (_health > 100)
            _health = 100;
        
        healthChangedEvent.Invoke(_health);
    }

    public override void Hit(Collision collision, Shell shell)
    {
        Vector3 collisionNormal = collision.GetContact(0).normal;
        float angle = Vector3.Angle(shell.transform.forward, -collisionNormal);
        Collider collider = collision.collider;
        if (ShouldBounce())
        {
            Reflect();
        }
        else
        {
            GetDamagedByShell();
        }

        bool ShouldBounce()
        {
            if (angle > shell.bounceAngle)
                return true;
            else return false;
        }

        void Reflect()
        {
            shell.transform.forward = Vector3.Reflect(shell.transform.forward, collisionNormal);
            shell.GetComponent<Rigidbody>().velocity = shell.transform.forward * shell.GetComponent<Rigidbody>().velocity.magnitude;
            shell.damage *= 0.7f;
            bounceEvent?.Invoke();
        }

        void GetDamagedByShell()
        {
            float damageAmount = CountDamage();
            if (damageAmount == 0)
            {
                bounceEvent?.Invoke();
            }
            else
            {
                hitEvent?.Invoke(damageAmount);
            }

            GetDamaged(damageAmount);
            shell.gameObject.SetActive(false);
        }

        float CountDamage()
        {

            float damage = shell.damage * (float)Math.Cos(angle / 180 * (float)Math.PI) * armor[collider];
            if (damage > 10)
                return damage;
            else
                return 0;
        }
    }

    private void GetDamaged(float damageAmount)
    {
        _health -= damageAmount;
        healthChangedEvent.Invoke(_health);
        if (_health <= 0)
            OnTankDestroy();
    }

    private void OnTankDestroy()
    {
        ExplosionManager.instance.Explode(transform.position + Vector3.up);
        destroyedEvent?.Invoke();
    }

    private void Move()
    {
        // Move back or forward
        rigidbody.AddRelativeForce(Vector3.forward * tankInfo.acceleration * tankController.verticalInput);
        
        // Turn left of right
        transform.Rotate(Vector3.up, tankController.horizontalInput * tankInfo.turnSpeed * Time.fixedDeltaTime);

        if (tankController.verticalInput != 0 || tankController.horizontalInput != 0)
        {
            if (!driveFlag)
            {
                driveFlag = true;
                driveStartEvent?.Invoke();
            }
        }
        else
        {
            if (driveFlag)
            {
                driveFlag = false;
                driveEndEvent?.Invoke();
            }
        }
    }

    private void Recharge()
    {
        // Recharge is considered completed, when rechargeProcess equals 1
        if (rechargeProcess != 1)
        {
            rechargeProcess = rechargeStopwatch.ElapsedMilliseconds /  1000f / tankInfo.rechargeDuration;
            // Commit recharge completion
            if (rechargeProcess >= 1)
            {
                rechargeProcess = 1;
                rechargeStopwatch.Reset();
            }
        }
    }

    private void Fire()
    {
        GameObject shell;
        // Fire when recharge completed and fireFlag requested
        if (fireFlag && rechargeProcess == 1)
        {
            ActivateShell();
            rechargeProcess = 0;
            rechargeStopwatch.Start();
            fireEvent?.Invoke();
        }

        // Reset flag of fireFlag request always
        if (fireFlag)
            fireFlag = false;
        
        void ActivateShell()
        {
            shell = ObjectPooler.ShellPooler.GetPooledObject();
            if (shell != null)
            {
                shell.transform.position = firePoint.position;
                shell.transform.rotation = turret.rotation;
                var shellScript = shell.GetComponent<Shell>();
                shellScript.GetComponent<Rigidbody>().velocity = shell.transform.forward * tankInfo.shellVelocity;
                shellScript.damage = tankInfo.cannonPower;
                shellScript.bounceAngle = 67;
                shell.SetActive(true);
                foreach (var bodyCollider in GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(shell.GetComponentInChildren<Collider>(), bodyCollider);
                    shellScript.ignored.Add(bodyCollider);
                }
            }
        }
    }

    private void RestrictMaxSpeed()
    {
        if (rigidbody.velocity.magnitude > tankInfo.maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * tankInfo.maxSpeed;
        }
    }

    private void RotateTurret()
    {
        Vector3 rotationDestination, rotationVector;
        if (tankController.aimPosition != null)
        {
            rotationDestination = new Vector3(tankController.aimPosition.x, physicalTurret.position.y, tankController.aimPosition.z);
            rotationVector = (rotationDestination - physicalTurret.position).normalized;
            // If distance between direction to mouse position (rotationVector) and barrel direction (-physicalTurret.up)
            // is more than very little, then execute rotation
            if (Vector3.Distance(rotationVector, -physicalTurret.up) > 0.001f)
            {
                ActuallyRotate();
                if (!turretRotateFlag)
                {
                    turretRotateFlag = true;
                    turretStartEvent?.Invoke();
                }
            }
            else
            {
                if (turretRotateFlag)
                {
                    turretRotateFlag = false;
                    turretEndEvent?.Invoke();
                }
            }
        }

        void ActuallyRotate()
        {
            float turnSpeed;
            float vectorDistance = Vector3.Distance(rotationVector, -physicalTurret.up);
            // If distance is quite short, then use low turret turn speed
            // else use usual turret turn speed
            if (vectorDistance < 0.01f)
                turnSpeed = 3;
            else if (vectorDistance < 0.04f)
                turnSpeed = 10;
            else
                turnSpeed = tankInfo.turretTurnSpeed;
            
            // Define the side where to turn turret
            float angle = Vector3.SignedAngle(-physicalTurret.up, rotationVector, Vector3.up);
            int sign = angle > 0 ? 1 : -1;

            physicalTurret.Rotate(Vector3.forward, sign * turnSpeed * Time.fixedDeltaTime);
        }
    }
}

[Serializable]
public class UnityFloatEvent : UnityEvent<float>
{
}

[Serializable]
public struct ColliderNumberPair
{
    public Collider collider;
    public float number;
}
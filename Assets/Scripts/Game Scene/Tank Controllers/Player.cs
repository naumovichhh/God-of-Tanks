using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Player : TankController
{
    public UnityEvent gameOver;
    public HealthBar healthBar;
    public RechargeBar rechargeBar;
    public RedDamageScreen redDamageScreen;
    public GameObject aim;
    private Tank tank;
    private float distance = 5;

    private void Awake()
    {
        tank = GetComponent<Tank>();
        tank.destroyedEvent += OnTankDestroy;
        tank.hitEvent += OnHit;
    }

    private void Start()
    {
        aim.transform.SetSiblingIndex(100);
        gameOver.AddListener(GameManager.Instance.OnGameOver);
    }

    // Read all inputs and update recharge bar
    private void Update()
    {
        HandleInput();
    }

    private void LateUpdate()
    {
        HandleUI();
    }

    private void OnHit(float damage)
    {
        float heaviness = damage / 50;
        heaviness = heaviness <= 1 ? heaviness : 1;
        float alpha = (float)Math.Sqrt(heaviness) * .4f;
        redDamageScreen.Show(alpha);
    }

    private void OnTankDestroy()
    {    
        DeactivatePhysical();
        gameOver.Invoke();
    }

    private void DeactivatePhysical()
    {
        Destroy(GetComponent<Rigidbody>());
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        GetComponent<PlayerSounds>().Detach();
        this.enabled = false;
        GetComponent<Tank>().enabled = false;
    }

    public void HealthChanged(float health)
    {
        if (healthBar != null)
            healthBar.SetHealth(health);
    }

    private void HandleInput()
    {
        fireInput = Input.GetAxis("Fire1");
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        if (verticalInput < -0.01f)
            horizontalInput = -horizontalInput;
        aimPosition = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void HandleUI()
    {
        if (rechargeBar != null)
            rechargeBar.SetValue(tank.rechargeProcess);
        
        SetAim();
    }

    private void SetAim()
    {
        /*float distance = (mousePosition - new Vector3(tank.turret.position.x, mousePosition.y, tank.turret.position.z)).magnitude;
        Vector3 aimPosition = tank.turret.position + tank.turret.forward * distance;
        aim.GetComponent<RectTransform>().localPosition = new Vector3(aimPosition.x, aimPosition.z, 0);*/

        float framesToReachMouse = tank.turretRotationAngle / tank.turretTurnSpeed / Time.deltaTime;
        float distanceToMouse = (base.aimPosition - new Vector3(tank.turret.position.x, base.aimPosition.y, tank.turret.position.z)).magnitude;
        if (framesToReachMouse > 1)
            distance = distance + (distanceToMouse - distance) / framesToReachMouse;
        else
            distance = distanceToMouse;
        Vector3 aimPosition = tank.turret.position + tank.turret.forward * distance;
        aim.GetComponent<RectTransform>().localPosition = new Vector3(aimPosition.x, aimPosition.z, 0);
    }
}

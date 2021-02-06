using System.Collections;
using UnityEngine;

public class EnemyController : TankController
{
    [SerializeField] private GameObject healthBar;
    private Vector3 moveDestination;
    private float xRange = 13;
    private float zRange = 6;
    private bool arrived;
    private Tank tank;
    private HealthBar privateHealthBar;

    private void Awake()
    {
        tank = GetComponent<Tank>();
        tank.destroyed.AddListener(OnTankDestroy);
    }

    private void OnEnable()
    {
        moveDestination = transform.position;
        // at spawn enemy tank won't move to another
        // point immediately
        arrived = true;
        // Move destination and aim point will be updated
        // with a certain interval
        StartCoroutine(MoveDestinationCoroutine());
        StartCoroutine(TurretDestinationCoroutine());
        privateHealthBar = ObjectPooler.HealthbarPooler.GetPooledObject().GetComponent<HealthBar>();
        privateHealthBar.gameObject.SetActive(true);
        //privateHealthBar = Instantiate(healthBar, GameObject.Find("World Space Canvas").transform).GetComponent<HealthBar>();
    }

    private void Update()
    {
        ManageMove();
        // Enemy tank evokes fire constantly
        fireEvent.Invoke();
    }

    private void LateUpdate()
    {
        privateHealthBar.GetComponent<RectTransform>().localPosition = new Vector3(transform.position.x, transform.position.z, 0);
    }

    private void OnTankDestroy()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        privateHealthBar.gameObject.SetActive(false);
    }

    public void HealthChanged(float health)
    {
        privateHealthBar.SetHealth(health);
    }

    private void ManageMove()
    {
        verticalInput = 0;
        horizontalInput = 0;
        // Checks if enemy has arrived at the destination place
        if (Vector3.Distance(moveDestination, transform.position)>0.5f && !arrived)
        {
            // Enemy hasn't arrive at the destination place, so it should move
            Vector3 moveVector = (moveDestination - transform.position).normalized;
            // Checks if moveVector and transform.forward match approximately
            if (Vector3.Distance(moveVector, transform.forward) > 0.05f)
            {
                // Checks whether moveVector and transform.back (virtual) match approximately
                if (Vector3.Distance(moveVector, -transform.forward) < 0.05f)
                {
                    // MoveVector and transform.back (virtual) match approximately, so enemy can move back
                    verticalInput = -1;
                }
                else
                {
                    // MoveVector and transform.back (virtual) don't match approximately, so enemy should rotate
                    float angle = Vector3.SignedAngle(transform.forward, moveVector, Vector3.up);
                    // Checks if it is faster to turn forward or back
                    if (System.Math.Abs(angle) < 90)
                    {
                        // It is faster to turn forward
                        horizontalInput = angle > 0 ? 1 : -1;
                    }
                    else
                    {
                        // It is faster to turn back
                        horizontalInput = angle < 0 ? 1 : -1;
                    }
                }
            }
            else
            {
                // MoveVector and transform.forward match approximately, so enemy can move forward
                verticalInput = 1;
            }
        }
        else
        {
            // Enemy arrived at the destination place, so arrived variable can be set
            arrived = true;
        }
    }

    // Update aiming point at a certain time interval
    private IEnumerator TurretDestinationCoroutine()
    {
        while (true)
        {
            mousePosition = new Vector3(Random.Range(-xRange, xRange), 0, Random.Range(-zRange, zRange));
            yield return new WaitForSeconds(Random.Range(tank.rechargeDurationRead, tank.rechargeDurationRead + 2));
        }
    }

    // Update move destination at a certain time interval
    private IEnumerator MoveDestinationCoroutine()
    {
        while (true)
        {
            moveDestination = new Vector3(Random.Range(-xRange, xRange), transform.position.y, Random.Range(-zRange, zRange));
            arrived = false;
            yield return new WaitForSeconds(Random.Range(4f, 20f));
        }
    }
}

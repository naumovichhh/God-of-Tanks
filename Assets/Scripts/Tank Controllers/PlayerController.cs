using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : TankController
{
    public HealthBar healthBar;
    public RechargeBar rechargeBar;
    public GameObject aim;
    private Tank tank;
    private float distance = 5;

    private void Awake()
    {
        tank = GetComponent<Tank>();
        tank.destroyed.AddListener(OnTankDestroy);
    }

    private void Start()
    {
        aim.transform.SetSiblingIndex(100);
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

    private void OnTankDestroy()
    {
        SceneManager.LoadScene("Game over");
    }

    public void HealthChanged(float health)
    {
        if (healthBar != null)
            healthBar.SetHealth(health);
    }

    private void HandleInput()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        if (verticalInput < -0.01f)
            horizontalInput = -horizontalInput;
        aimPosition = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(0))
            fireEvent.Invoke();
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

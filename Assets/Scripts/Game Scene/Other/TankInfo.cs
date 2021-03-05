using UnityEngine;

[CreateAssetMenu(fileName = "TankInfo", menuName = "ScriptableObjects/TankInfo", order = 1)]
public class TankInfo : ScriptableObject
{
    public float turretTurnSpeed;
    public float cannonPower;
    public float maxSpeed;
    public float acceleration;
    public float turnSpeed;
    public float rechargeDuration;
    public float shellVelocity;
}

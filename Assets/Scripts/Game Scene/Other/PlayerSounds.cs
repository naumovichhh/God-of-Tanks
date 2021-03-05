using System;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioSource turretSource;
    [SerializeField] private AudioSource bounceSource;
    [SerializeField] private AudioSource fireSource;
    [SerializeField] private AudioSource hitSource;
    [SerializeField] private AudioSource driveSource;
    [SerializeField] private AudioSource destroyedSource;
    private Tank tank;

    private void Awake()
    {
        tank = GetComponent<Tank>();
        tank.fireEvent += OnFire;
        tank.hitEvent += OnHit;
        tank.turretStartEvent += OnTurretStart;
        tank.turretEndEvent += OnTurretEnd;
        tank.driveStartEvent += OnDriveStart;
        tank.driveEndEvent += OnDriveEnd;
        tank.destroyedEvent += OnDestroyed;
        tank.bounceEvent += OnRicochette;
    }

    public void Detach()
    {
        turretSource.Stop();
        driveSource.Stop();
        tank.fireEvent -= OnFire;
        tank.hitEvent -= OnHit;
        tank.turretStartEvent -= OnTurretStart;
        tank.turretEndEvent -= OnTurretEnd;
        tank.driveStartEvent -= OnDriveStart;
        tank.driveEndEvent -= OnDriveEnd;
        tank.destroyedEvent -= OnDestroyed;
        tank.bounceEvent -= OnRicochette;
    }

    private void OnFire()
    {
        fireSource.Play();
    }

    private void OnHit(float damage)
    {
        
        float heaviness = damage / 50;
        heaviness = heaviness <= 1 ? heaviness : 1;
        heaviness = (float)Math.Sqrt(heaviness);
        hitSource.volume = heaviness;
        hitSource.Play();
    }

    private void OnRicochette()
    {
        bounceSource.Play();
    }

    private void OnTurretStart()
    {
        turretSource.Play();
    }

    private void OnTurretEnd()
    {
        turretSource.Stop();
    }

    private void OnDriveStart()
    {
        driveSource.Play();
    }

    private void OnDriveEnd()
    {
        driveSource.Stop();
    }

    private void OnDestroyed()
    {
        destroyedSource.Play();
    }
}

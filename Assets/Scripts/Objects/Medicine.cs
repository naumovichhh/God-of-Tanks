using UnityEngine;

public class Medicine : MonoBehaviour, ObjectToSpawn
{
    private void OnTriggerEnter(Collider other)
    {
        var tank = other.GetComponentInParent<Tank>();
        if (tank != null)
        {
            tank.Healed(40);
            gameObject.SetActive(false);
        }
    }

    public bool IsOverlapped()
    {
        if (Physics.OverlapSphere(transform.position, transform.lossyScale.y / 2).Length > 1)
            return true;
        else
            return false;
    }
}

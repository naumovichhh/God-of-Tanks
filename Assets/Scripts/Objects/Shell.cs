using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public float speed;
    public float damage;
    public float bounceAngle;
    public List<Collider> ignored = new List<Collider>();

    private void OnEnable()
    {
        foreach (var collider in ignored)
        {
            Physics.IgnoreCollision(GetComponentInChildren<Collider>(), collider, false);
        }

        ignored.Clear();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        var shellCollider = collider.gameObject.GetComponentInParent<ShellCollider>();
        if (shellCollider != null)
        {
            shellCollider.Hit(collision, this);
        }
    }
}

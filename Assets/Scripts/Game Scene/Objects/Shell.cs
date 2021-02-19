using System.Collections;
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
        StartCoroutine(SetTimeout());
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

    private IEnumerator SetTimeout()
    {
        yield return new WaitForSeconds(7f);
        gameObject.SetActive(false);
    }
}

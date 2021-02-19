using UnityEngine;

public class ShellCollider : MonoBehaviour
{
    public virtual void Hit(Collision collision, Shell shell)
    {
        shell.gameObject.SetActive(false);
    }
}

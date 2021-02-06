using UnityEngine;
using UnityEngine.Events;

public abstract class TankController : MonoBehaviour
{
    public float horizontalInput { get; protected set; }
    public float verticalInput { get; protected set; }
    public Vector3 mousePosition { get; protected set; }
    public UnityEvent fireEvent;
}

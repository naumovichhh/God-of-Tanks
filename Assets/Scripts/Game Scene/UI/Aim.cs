using UnityEngine;

public class Aim : MonoBehaviour
{
    private void Start()
    {
        OnCameraSizeChanged();
    }

    public void OnCameraSizeChanged()
    {
        float scale = 600f / UnityEngine.Camera.main.pixelHeight;
        transform.localScale = new Vector3(scale, scale, 1);
    }
}

using UnityEngine;

public class CursorImage : MonoBehaviour
{
    public Texture2D texture;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.SetCursor(texture, new Vector2(100, 100), CursorMode.Auto);
    }

    private void OnDestroy()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}

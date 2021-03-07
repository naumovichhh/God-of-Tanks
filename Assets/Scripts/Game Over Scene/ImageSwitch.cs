using UnityEngine;
using UnityEngine.UI;

public class ImageSwitch : MonoBehaviour
{
    public Sprite winSprite;
    public Sprite lossSprite;

    private void Start()
    {
        Image image = GetComponent<Image>();
        if (GameManager.Instance.Won)
        {
            image.sprite = winSprite;
        }
        else
        {
            image.sprite = lossSprite;
        }
    }
}

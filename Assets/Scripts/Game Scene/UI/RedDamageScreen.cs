using System;
using UnityEngine;
using UnityEngine.UI;

public class RedDamageScreen : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.canvasRenderer.SetAlpha(0);
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    }
    public void Show(float alpha)
    {
        image.canvasRenderer.SetAlpha(alpha);
        image.CrossFadeAlpha(0, .5f, false);
    }
}

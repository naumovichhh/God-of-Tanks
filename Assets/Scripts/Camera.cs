﻿using UnityEngine;

public class Camera : MonoBehaviour
{
    private float screenWidth;
    private float screenHeight;

    private void Start()
    {
        Update();
    }

    private void Update()
    {
        // Check recorded and current screen sizes for equality.
        // If screen size changed, then there is need to change
        // camera pixelRect, because camera field of view
        // should always fit the game field (16:9)
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            AssignWidthAndHeight();
            var camera = GetComponent<UnityEngine.Camera>();
            if ((float)Screen.width / (float)Screen.height > 1367f / 768f)
            {
                CutWidth(camera);
            }
            else if ((float)Screen.width / (float)Screen.height < 1365f / 768f)
            {
                CutHeight(camera);
            }
            else
            {
                camera.pixelRect = new Rect(0, 0, Screen.width, Screen.height);
            }
        }
    }

    private void AssignWidthAndHeight()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
    }

    private void CutWidth(UnityEngine.Camera camera)
    {
        var width = Screen.height * 16 / 9;
        var offset = (Screen.width - width) / 2;
        camera.pixelRect = new Rect(offset, 0, width, Screen.height);
    }

    private void CutHeight(UnityEngine.Camera camera)
    {
        var height = Screen.width * 9 / 16;
        var offset = (Screen.height - height) / 2;
        camera.pixelRect = new Rect(0, offset, Screen.width, height);
    }
}

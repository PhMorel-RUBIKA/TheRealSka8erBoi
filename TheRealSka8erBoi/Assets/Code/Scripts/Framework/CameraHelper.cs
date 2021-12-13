using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHelper : MonoBehaviour
{
    public Camera elCamera;
    public float pixelPerUnit = 24f;
    public float zoom = 240f;
    public bool usePixelScale = false;
    public float pixelScale = 4f; 
    
    Vector3 cameraPos = Vector3.zero;

    public void Move(Vector3 dir)
    {
        ApplyZoom();
        cameraPos += dir; 
        AdjustCamera();
    }

    public void MoveTo(Vector3 pos)
    {
        ApplyZoom();
        cameraPos = pos; 
        AdjustCamera();
    }
    
    public void AdjustCamera()
    {
        elCamera.transform.position =
            new Vector3(RoundToNearestPixel(cameraPos.x), RoundToNearestPixel(cameraPos.y), -10f);
    }
    
    public float RoundToNearestPixel(float pos)
    {
        float screenPixelPerUnit = Screen.height / (elCamera.orthographicSize * 2f);
        float pixelValue = Mathf.Round(pos * screenPixelPerUnit);

        return pixelValue / screenPixelPerUnit;
    }

    public void ApplyZoom()
    {
        if (!usePixelScale)
        {
            float smallestDimension = Screen.height < Screen.width ? Screen.height : Screen.width;
            pixelScale = Mathf.Round(smallestDimension / zoom);
        }

        elCamera.orthographicSize = (Screen.height / (pixelPerUnit * pixelScale)) * 0.5f;
    }

    public Vector3 GetCameraPos()
    {
        return cameraPos;
    }
}

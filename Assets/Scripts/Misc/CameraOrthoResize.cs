using UnityEngine;

public class CameraOrthoResize : MonoBehaviour
{
    readonly float _16x9Ratio = 16 / 9f;

    void Awake()
    {
        OrthographicResize();
    }

    void OrthographicResize()
    {
        float currentAspectRatio = Screen.width / (float)Screen.height;

        if(currentAspectRatio < _16x9Ratio)
        {
            float difference = _16x9Ratio / currentAspectRatio;

            Camera.main.orthographicSize *= difference;
        }
    }
}

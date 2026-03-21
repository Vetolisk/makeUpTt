using UnityEngine;

public class FixedWorldCamera : MonoBehaviour
{
    [Header("World Size")]
    [SerializeField] private float worldWidth = 10f;   // ширина видимой области в мире
    [SerializeField] private float worldHeight = 18f;  // высота видимой области в мире

    [Header("Reference Resolution")]
    [SerializeField] private float referenceWidth = 1080f;
    [SerializeField] private float referenceHeight = 1920f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        mainCamera.orthographic = true;

        AdjustCamera();
    }

    void AdjustCamera()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float worldAspect = worldWidth / worldHeight;

        if (screenAspect >= worldAspect)
        {
            // Экран шире - подстраиваем по высоте
            mainCamera.orthographicSize = worldHeight / 2f;
        }
        else
        {
            // Экран уже - подстраиваем по ширине
            mainCamera.orthographicSize = (worldWidth / 2f) / screenAspect;
        }

        Debug.Log($"Screen: {Screen.width}x{Screen.height}, Camera Size: {mainCamera.orthographicSize}");
    }
}
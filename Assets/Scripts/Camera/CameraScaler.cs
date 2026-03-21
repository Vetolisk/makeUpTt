using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    [Header("Reference Resolution")]
    [SerializeField] private float referenceWidth = 1080f;
    [SerializeField] private float referenceHeight = 1920f;

    private Camera mainCamera;
    private float originalSize;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        originalSize = mainCamera.orthographicSize;
        AdjustCamera();
    }

    void AdjustCamera()
    {
        float targetAspect = referenceWidth / referenceHeight;
        float currentAspect = (float)Screen.width / (float)Screen.height;

        // Подстраиваем размер камеры под ширину экрана
        float newSize = originalSize * (targetAspect / currentAspect);
        mainCamera.orthographicSize = Mathf.Max(newSize, originalSize);
    }
}
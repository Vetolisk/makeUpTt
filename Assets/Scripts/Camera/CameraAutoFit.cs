using UnityEngine;

public class CameraAutoFit : MonoBehaviour
{
    [Header("Boundaries (в координатах мира)")]
    [SerializeField] private float leftBoundary = -5f;    // левая граница
    [SerializeField] private float rightBoundary = 5f;    // правая граница
    [SerializeField] private float bottomBoundary = -4f;  // нижняя граница
    [SerializeField] private float topBoundary = 4f;      // верхняя граница

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        FitCameraToBounds();
    }

    void FitCameraToBounds()
    {
        // Вычисляем нужную высоту камеры, чтобы влезла ширина
        float worldWidth = rightBoundary - leftBoundary;
        float worldHeight = topBoundary - bottomBoundary;

        float screenAspect = (float)Screen.width / (float)Screen.height;
        float requiredHeightForWidth = worldWidth / screenAspect;

        // Выбираем больший размер, чтобы всё влезло
        float targetSize = Mathf.Max(worldHeight / 2f, requiredHeightForWidth / 2f);

        mainCamera.orthographicSize = targetSize;

        // Центрируем камеру
        float centerX = (leftBoundary + rightBoundary) / 2f;
        float centerY = (bottomBoundary + topBoundary) / 2f;
        transform.position = new Vector3(centerX, centerY, transform.position.z);

        Debug.Log($"Screen: {Screen.width}x{Screen.height}, Camera Size: {targetSize}");
    }
}
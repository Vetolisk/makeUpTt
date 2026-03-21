using UnityEngine;

public class ScreenAnchor : MonoBehaviour
{
    public enum AnchorPosition
    {
        BottomLeft,
        BottomCenter,
        BottomRight,
        CenterLeft,
        Center,
        CenterRight,
        TopLeft,
        TopCenter,
        TopRight
    }

    [Header("Anchor Settings")]
    [SerializeField] private AnchorPosition anchor = AnchorPosition.BottomLeft;
    [SerializeField] private Vector2 offset = new Vector2(50, 50); // отступ в пикселях
    [SerializeField] private bool usePixelOffset = true;

    [Header("Reference Resolution")]
    [SerializeField] private float referenceWidth = 1080f;
    [SerializeField] private float referenceHeight = 1920f;

    private Camera mainCamera;
    private Vector3 originalScale;

    void Start()
    {
        mainCamera = Camera.main;
        originalScale = transform.localScale;
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (mainCamera == null) return;

        // Получаем размеры экрана в пикселях
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Вычисляем масштаб относительно эталонного разрешения
        float scaleX = screenWidth / referenceWidth;
        float scaleY = screenHeight / referenceHeight;

        // Базовые координаты в пикселях
        Vector2 pixelPos = Vector2.zero;

        switch (anchor)
        {
            case AnchorPosition.BottomLeft:
                pixelPos = new Vector2(offset.x, offset.y);
                break;
            case AnchorPosition.BottomCenter:
                pixelPos = new Vector2(screenWidth / 2, offset.y);
                break;
            case AnchorPosition.BottomRight:
                pixelPos = new Vector2(screenWidth - offset.x, offset.y);
                break;
            case AnchorPosition.CenterLeft:
                pixelPos = new Vector2(offset.x, screenHeight / 2);
                break;
            case AnchorPosition.Center:
                pixelPos = new Vector2(screenWidth / 2, screenHeight / 2);
                break;
            case AnchorPosition.CenterRight:
                pixelPos = new Vector2(screenWidth - offset.x, screenHeight / 2);
                break;
            case AnchorPosition.TopLeft:
                pixelPos = new Vector2(offset.x, screenHeight - offset.y);
                break;
            case AnchorPosition.TopCenter:
                pixelPos = new Vector2(screenWidth / 2, screenHeight - offset.y);
                break;
            case AnchorPosition.TopRight:
                pixelPos = new Vector2(screenWidth - offset.x, screenHeight - offset.y);
                break;
        }

        // Если используем пиксельные отступы, масштабируем их
        if (usePixelOffset)
        {
            pixelPos.x = offset.x * scaleX;
            pixelPos.y = offset.y * scaleY;

            switch (anchor)
            {
                case AnchorPosition.BottomLeft:
                    pixelPos = new Vector2(pixelPos.x, pixelPos.y);
                    break;
                case AnchorPosition.BottomRight:
                    pixelPos = new Vector2(screenWidth - pixelPos.x, pixelPos.y);
                    break;
                case AnchorPosition.TopLeft:
                    pixelPos = new Vector2(pixelPos.x, screenHeight - pixelPos.y);
                    break;
                case AnchorPosition.TopRight:
                    pixelPos = new Vector2(screenWidth - pixelPos.x, screenHeight - pixelPos.y);
                    break;
            }
        }

        // Конвертируем пиксели в мировые координаты
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(pixelPos.x, pixelPos.y, 10));
        transform.position = new Vector3(worldPos.x, worldPos.y, transform.position.z);

        // Масштабируем объект под разрешение (опционально)
        // transform.localScale = originalScale * Mathf.Min(scaleX, scaleY);
    }

    // Обновляем позицию при изменении разрешения (для телефона)
    void Update()
    {
        UpdatePosition();
    }
}
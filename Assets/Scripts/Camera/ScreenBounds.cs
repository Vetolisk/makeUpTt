using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    public static ScreenBounds Instance { get; private set; }

    public float Left { get; private set; }
    public float Right { get; private set; }
    public float Bottom { get; private set; }
    public float Top { get; private set; }

    public float Width { get; private set; }
    public float Height { get; private set; }

    private Camera mainCamera;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        mainCamera = Camera.main;
        UpdateBounds();
    }

    void UpdateBounds()
    {
        if (mainCamera == null) return;

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        Left = bottomLeft.x;
        Right = topRight.x;
        Bottom = bottomLeft.y;
        Top = topRight.y;

        Width = Right - Left;
        Height = Top - Bottom;
    }

    public Vector3 ClampPosition(Vector3 position, float padding = 0.5f)
    {
        return new Vector3(
            Mathf.Clamp(position.x, Left + padding, Right - padding),
            Mathf.Clamp(position.y, Bottom + padding, Top - padding),
            position.z
        );
    }
}
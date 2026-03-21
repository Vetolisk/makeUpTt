using UnityEngine;

public class ItemPositioner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float padding = 0.5f;

    void Start()
    {
        // Размещаем предмет в безопасной зоне
        Reposition();
    }

    void Reposition()
    {
        if (ScreenBounds.Instance != null)
        {
            transform.position = ScreenBounds.Instance.ClampPosition(transform.position, padding);
        }
    }
}
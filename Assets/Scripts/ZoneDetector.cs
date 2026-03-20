using UnityEngine;
using UnityEngine.EventSystems;

public class ZoneDetector : MonoBehaviour
{
    [SerializeField] private Collider2D faceCollider;

    // Проверяет, находится ли позиция в зоне лица
    public bool IsPointerInZone(Vector2 screenPosition)
    {
        if (faceCollider == null)
        {
            Debug.LogWarning("Face collider not assigned!");
            return false;
        }

        // Конвертируем экранные координаты в мировые
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(screenPosition);

        // Проверяем попадание в коллайдер
        return faceCollider.OverlapPoint(worldPoint);
    }

    // Альтернативный метод для проверки через PointerEventData
    public bool IsPointerInZone(PointerEventData eventData)
    {
        return IsPointerInZone(eventData.position);
    }

    private void OnDrawGizmos()
    {
        if (faceCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(faceCollider.bounds.center, faceCollider.bounds.size);
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoneDetector : MonoBehaviour
{
    [Header("Face Zone")]
    [SerializeField] private Collider2D faceCollider;

    private void Start()
    {
        if (faceCollider == null)
            faceCollider = GetComponent<Collider2D>();
    }

    // Метод для проверки точки в зоне
    public bool IsPointInZone(Vector2 screenPosition)
    {
        if (faceCollider == null)
        {
            Debug.LogWarning("Face collider not assigned!");
            return false;
        }

        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(screenPosition);
        return faceCollider.OverlapPoint(worldPoint);
    }

    // Перегрузка для PointerEventData
    public bool IsPointInZone(PointerEventData eventData)
    {
        return IsPointInZone(eventData.position);
    }

    private void OnDrawGizmos()
    {
        if (faceCollider != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.3f);
            Gizmos.DrawCube(faceCollider.bounds.center, faceCollider.bounds.size);
        }
    }
}
using UnityEngine;

public class DragHandler : MonoBehaviour
{
    private Camera mainCamera;
    private HandController handController;
    private Vector3 offset;
    private bool isDragging = false;

    [Header("Settings")]
    [SerializeField] private float padding = 0.5f;

    void Start()
    {
        mainCamera = Camera.main;
        handController = GetComponent<HandController>();
    }

    void OnMouseDown()
    {
        if (handController == null) return;
        if (!handController.IsHoldingCream() && !handController.IsHoldingBrush() && !handController.IsHoldingLipstick()) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        offset = transform.position - mousePos;
        isDragging = true;

        handController.StartDrag();
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;
        if (handController == null) return;
        if (!handController.IsHoldingCream() && !handController.IsHoldingBrush() && !handController.IsHoldingLipstick()) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 newPosition = mousePos + offset;

        // Ограничиваем движение границами экрана
        if (ScreenBounds.Instance != null)
        {
            newPosition = ScreenBounds.Instance.ClampPosition(newPosition, padding);
        }

        transform.position = newPosition;
    }

    void OnMouseUp()
    {
        if (!isDragging) return;
        if (handController == null) return;
        if (!handController.IsHoldingCream() && !handController.IsHoldingBrush() && !handController.IsHoldingLipstick()) return;

        isDragging = false;

        var fakeEvent = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        fakeEvent.position = Input.mousePosition;

        handController.EndDrag(fakeEvent);
    }
}
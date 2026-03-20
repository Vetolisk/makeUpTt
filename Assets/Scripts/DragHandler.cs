using UnityEngine;

public class DragHandler : MonoBehaviour
{
    private Camera mainCamera;
    private HandController handController;
    private Vector3 offset;
    private bool isDragging = false;

    void Start()
    {
        mainCamera = Camera.main;
        handController = GetComponent<HandController>();
    }

    void OnMouseDown()
    {
        if (handController == null) return;
        if (!handController.IsHoldingCream()) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        offset = transform.position - mousePos;
        isDragging = true;

        handController.StartDrag();
        Debug.Log("Drag started");
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;
        if (handController == null) return;
        if (!handController.IsHoldingCream()) return;

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos + offset;
    }

    void OnMouseUp()
    {
        if (!isDragging) return;
        if (handController == null) return;
        if (!handController.IsHoldingCream()) return;

        isDragging = false;

        var fakeEvent = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        fakeEvent.position = Input.mousePosition;

        handController.EndDrag(fakeEvent);
        Debug.Log("Drag ended");
    }
}
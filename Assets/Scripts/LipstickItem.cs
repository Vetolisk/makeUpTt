using UnityEngine;

public class LipstickItem : MonoBehaviour
{
    [Header("Lipstick Settings")]
    [SerializeField] private Sprite lipstickSprite; // спрайт губ с этой помадой

    private HandController handController;

    void Start()
    {
        handController = FindObjectOfType<HandController>();
    }

    void OnMouseDown()
    {
        if (handController == null)
        {
            Debug.LogError("HandController not found");
            return;
        }

        if (handController.IsHoldingCream() || handController.IsHoldingBrush() || handController.IsHoldingLipstick())
        {
            Debug.Log("Hand is busy");
            return;
        }

        Debug.Log("Lipstick clicked: " + gameObject.name);

        if (handController == null) return;

        // Проверка, не занята ли рука
        if (handController.IsHandBusy())
        {
            Debug.Log("Hand is busy with another item!");
            return;
        }
        handController.TakeLipstick(transform, lipstickSprite);
    }
}
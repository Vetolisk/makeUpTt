using UnityEngine;

public class CreamItem : MonoBehaviour
{
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

        if (handController.IsHoldingCream() || handController.IsHoldingBrush())
        {
            Debug.Log("Hand is busy");
            return;
        }

        if (handController == null) return;

        // Проверка, не занята ли рука
        if (handController.IsHandBusy())
        {
            Debug.Log("Hand is busy with another item!");
            return;
        }


        Debug.Log("Cream clicked");
        handController.TakeCream();
    }
}
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
        Debug.Log("Cream clicked");

        if (handController != null && !handController.IsHoldingCream())
        {
            handController.TakeCream();
        }
    }
}
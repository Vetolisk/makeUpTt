using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    [Header("Shadow Data")]
    [SerializeField] private Color brushColor = Color.red;
    [SerializeField] private Sprite eyeshadowSprite;

    [Header("References")]
    [SerializeField] private HandController handController;

    void Start()
    {
        if (handController == null)
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

        handController.PickColor(brushColor, eyeshadowSprite, transform.position);
    }
}
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

        if (eyeshadowSprite == null)
            Debug.LogError("Eyeshadow sprite not assigned in ColorPicker on " + gameObject.name);
    }

    void OnMouseDown()
    {
        Debug.Log("ColorPicker clicked: " + gameObject.name);

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

        if (eyeshadowSprite == null)
        {
            Debug.LogError("Cannot pick color: eyeshadowSprite is null in " + gameObject.name);
            return;
        }

        if (handController == null) return;

        // Проверка, не занята ли рука
        if (handController.IsHandBusy())
        {
            Debug.Log("Hand is busy with another item!");
            return;
        }

        Debug.Log("Picking color with sprite: " + eyeshadowSprite.name);
        handController.PickColor(brushColor, eyeshadowSprite, transform.position);
    }
}
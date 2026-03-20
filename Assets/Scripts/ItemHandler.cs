using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHandler : MonoBehaviour, IPointerClickHandler
{
    [Header("Item Settings")]
    [SerializeField] private ItemType itemType;
    [SerializeField] private HandController handController;
    [SerializeField] private FaceController faceController;

    [Header("Item Positions")]
    [SerializeField] private Vector2 itemPosition;
    [SerializeField] private Vector2 holdPosition;

    private enum ItemType
    {
        Cream,
        Shadow,
        Lipstick
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Нельзя взять предмет, если рука уже занята
        if (handController.GetCurrentState() != HandController.HandState.Idle)
            return;

        TakeItem();
    }

    private void TakeItem()
    {
        // Анимация взятия предмета
        handController.AnimateTakeItem(itemPosition, () =>
        {
            // Устанавливаем состояние руки
            switch (itemType)
            {
                case ItemType.Cream:
                    handController.SetState(HandController.HandState.HoldingCream);
                    break;
                case ItemType.Shadow:
                    handController.SetState(HandController.HandState.HoldingBrush);
                    break;
                case ItemType.Lipstick:
                    handController.SetState(HandController.HandState.HoldingLipstick);
                    break;
            }

            // Перемещаем руку в позицию удержания (на уровень груди)
            LeanTween.move(handController.GetComponent<RectTransform>(), holdPosition, 0.2f);
        });

        // Подписываемся на событие успешного нанесения
        handController.OnMakeupApplied += ApplyMakeup;
    }

    private void ApplyMakeup()
    {
        switch (itemType)
        {
            case ItemType.Cream:
                faceController.ApplyCream();
                break;
            case ItemType.Shadow:
                faceController.ApplyShadow();
                break;
            case ItemType.Lipstick:
                faceController.ApplyLipstick();
                break;
        }

        // Отписываемся, чтобы не вызвать повторно
        handController.OnMakeupApplied -= ApplyMakeup;
    }
}
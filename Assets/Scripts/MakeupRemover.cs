using UnityEngine;
using UnityEngine.EventSystems;

public class MakeupRemover : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private FaceController faceController;
    [SerializeField] private HandController handController;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Стираем макияж
        faceController.ClearMakeup();

        // Возвращаем руку в исходное состояние
        handController.ResetToIdle();

        // Если рука в движении — останавливаем
        LeanTween.cancel(handController.gameObject);

        Debug.Log("Makeup removed with sponge!");
    }
}
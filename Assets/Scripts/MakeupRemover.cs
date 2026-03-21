using UnityEngine;

public class MakeupRemover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FaceController faceController;
    [SerializeField] private HandController handController;

    void Start()
    {
        if (faceController == null)
            faceController = FindObjectOfType<FaceController>();

        if (handController == null)
            handController = FindObjectOfType<HandController>();
    }

    void OnMouseDown()
    {
        Debug.Log("Sponge clicked - clearing all makeup");

        // Стираем макияж
        if (faceController != null)
            faceController.ClearAllMakeup();

        // Сбрасываем руку в idle состояние (если она что-то держит)
        if (handController != null)
            handController.ResetToIdle();

        // Небольшая визуальная анимация спонжика (опционально)
        StartCoroutine(AnimateSpongeClick());
    }

    private System.Collections.IEnumerator AnimateSpongeClick()
    {
        Vector3 originalScale = transform.localScale;

        // Сжимаем
        float elapsed = 0;
        while (elapsed < 0.05f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / 0.05f;
            transform.localScale = Vector3.Lerp(originalScale, originalScale * 0.8f, t);
            yield return null;
        }

        // Возвращаем
        elapsed = 0;
        while (elapsed < 0.05f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / 0.05f;
            transform.localScale = Vector3.Lerp(originalScale * 0.8f, originalScale, t);
            yield return null;
        }

        transform.localScale = originalScale;
    }
}
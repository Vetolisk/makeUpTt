using UnityEngine;

public class FaceController : MonoBehaviour
{
    [Header("Основной спрайт персонажа")]
    [SerializeField] private SpriteRenderer characterSprite;

    [Header("Спрайт прыщей (отдельно)")]
    [SerializeField] private SpriteRenderer acneSprite;

    [Header("Настройки")]
    [SerializeField] private bool hasAcne = true;

    void Start()
    {
        // Находим компоненты, если не назначены
        if (characterSprite == null)
            characterSprite = GetComponent<SpriteRenderer>();

        if (acneSprite == null)
        {
            // Ищем дочерний объект с именем "Acne"
            Transform acneTransform = transform.Find("Acne");
            if (acneTransform != null)
                acneSprite = acneTransform.GetComponent<SpriteRenderer>();
        }

        // Устанавливаем начальное состояние
        SetAcneVisible(hasAcne);
    }

    // Убираем прыщи
    public void RemoveAcne()
    {
        if (!hasAcne) return;

        hasAcne = false;
        SetAcneVisible(false);
        Debug.Log("Прыщи исчезли!");
    }

    // Показать/скрыть прыщи
    private void SetAcneVisible(bool visible)
    {
        if (acneSprite != null)
            acneSprite.enabled = visible;
    }

    // Проверка, есть ли прыщи
    public bool HasAcne() => hasAcne;
}
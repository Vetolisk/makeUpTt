using UnityEngine;

public class FaceController : MonoBehaviour
{
    [Header("Sprite Renderers")]
    [SerializeField] private SpriteRenderer bodyRenderer;   // тело (чистое лицо)
    [SerializeField] private SpriteRenderer acneRenderer;   // прыщи

    [Header("Makeup Overlays (опционально)")]
    [SerializeField] private GameObject eyeshadowOverlay;   // тени
    [SerializeField] private GameObject lipstickOverlay;    // помада

    private bool hasAcne = true;
    private bool hasShadow = false;
    private bool hasLipstick = false;

    private void Start()
    {
        // Находим компоненты, если не назначены
        if (bodyRenderer == null)
            bodyRenderer = transform.Find("Body")?.GetComponent<SpriteRenderer>();
        if (acneRenderer == null)
            acneRenderer = transform.Find("Acne")?.GetComponent<SpriteRenderer>();

        // Начальное состояние: прыщи включены
        SetAcneVisible(true);

        // Выключаем макияж
        if (eyeshadowOverlay != null) eyeshadowOverlay.SetActive(false);
        if (lipstickOverlay != null) lipstickOverlay.SetActive(false);
    }

    // Крем — убираем прыщи
    public void ApplyCream()
    {
        if (!hasAcne) return;

        hasAcne = false;
        SetAcneVisible(false);
        Debug.Log("Cream applied - acne removed!");
    }

    // Тени
    public void ApplyShadow()
    {
        if (hasShadow) return;

        hasShadow = true;
        if (eyeshadowOverlay != null)
            eyeshadowOverlay.SetActive(true);
        Debug.Log("Shadow applied!");
    }

    // Помада
    public void ApplyLipstick()
    {
        if (hasLipstick) return;

        hasLipstick = true;
        if (lipstickOverlay != null)
            lipstickOverlay.SetActive(true);
        Debug.Log("Lipstick applied!");
    }

    // Спонжик — стираем всё
    public void ClearMakeup()
    {
        hasAcne = true;
        hasShadow = false;
        hasLipstick = false;

        SetAcneVisible(true);

        if (eyeshadowOverlay != null) eyeshadowOverlay.SetActive(false);
        if (lipstickOverlay != null) lipstickOverlay.SetActive(false);

        Debug.Log("All makeup cleared!");
    }

    private void SetAcneVisible(bool visible)
    {
        if (acneRenderer != null)
            acneRenderer.enabled = visible;
    }

    // Проверка состояний (для других скриптов)
    public bool HasAcne() => hasAcne;
    public bool HasShadow() => hasShadow;
    public bool HasLipstick() => hasLipstick;
}
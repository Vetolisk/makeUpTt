using UnityEngine;

public class FaceController : MonoBehaviour
{
    [Header("Sprite Renderers")]
    [SerializeField] private SpriteRenderer characterSprite;
    [SerializeField] private SpriteRenderer acneSprite;
    [SerializeField] private SpriteRenderer eyeshadowSprite;

    private bool hasAcne = true;
    private bool hasShadow = false;

    void Start()
    {
        if (characterSprite == null)
            characterSprite = GetComponent<SpriteRenderer>();

        if (acneSprite == null)
        {
            Transform acneTransform = transform.Find("Acne");
            if (acneTransform != null)
                acneSprite = acneTransform.GetComponent<SpriteRenderer>();
        }

        if (eyeshadowSprite == null)
        {
            Transform shadowTransform = transform.Find("Eyeshadow");
            if (shadowTransform != null)
                eyeshadowSprite = shadowTransform.GetComponent<SpriteRenderer>();
            else
                Debug.LogError("Eyeshadow object not found on Character!");
        }

        SetAcneVisible(true);

        // ПРОВЕРКА: принудительно выключаем тени
        if (eyeshadowSprite != null)
        {
            eyeshadowSprite.enabled = false;
            Debug.Log("Eyeshadow disabled at start. Current enabled: " + eyeshadowSprite.enabled);
        }
    }

    public void RemoveAcne()
    {
        if (!hasAcne) return;
        hasAcne = false;
        SetAcneVisible(false);
        Debug.Log("Acne removed");
    }

    public void ApplyShadow(Sprite shadowSprite)
    {
        Debug.Log("=== ApplyShadow START ===");

        if (eyeshadowSprite == null)
        {
            Debug.LogError("eyeshadowSprite is NULL!");
            return;
        }

        if (shadowSprite == null)
        {
            Debug.LogError("shadowSprite is NULL!");
            return;
        }

        // Меняем спрайт
        eyeshadowSprite.sprite = shadowSprite;
        Debug.Log("Sprite changed to: " + shadowSprite.name);

        // ВКЛЮЧАЕМ объект
        eyeshadowSprite.enabled = true;
        Debug.Log("Eyeshadow enabled set to: " + eyeshadowSprite.enabled);

        hasShadow = true;

        // ПРОВЕРКА: принудительно проверяем
        if (eyeshadowSprite.enabled)
            Debug.Log("SUCCESS: Eyeshadow is now visible!");
        else
            Debug.LogError("FAILED: Eyeshadow is still disabled!");
    }

    public void ClearShadow()
    {
        if (hasShadow)
        {
            if (eyeshadowSprite != null)
                eyeshadowSprite.enabled = false;
            hasShadow = false;
            Debug.Log("Shadow cleared");
        }
    }

    public void ClearAllMakeup()
    {
        SetAcneVisible(true);
        if (eyeshadowSprite != null)
            eyeshadowSprite.enabled = false;
        hasAcne = true;
        hasShadow = false;
        Debug.Log("All makeup cleared");
    }

    private void SetAcneVisible(bool visible)
    {
        if (acneSprite != null)
            acneSprite.enabled = visible;
    }

    public bool HasAcne() => hasAcne;
    public bool HasShadow() => hasShadow;
}
using UnityEngine;

public class FaceController : MonoBehaviour
{
    [Header("Sprite Renderers")]
    [SerializeField] private SpriteRenderer characterSprite;
    [SerializeField] private SpriteRenderer acneSprite;
    [SerializeField] private SpriteRenderer eyeshadowSprite;
    [SerializeField] private SpriteRenderer lipsSprite;

    private bool hasAcne = true;
    private bool hasShadow = false;
    private bool hasLipstick = false;

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
        }

        if (lipsSprite == null)
        {
            Transform lipsTransform = transform.Find("Lips");
            if (lipsTransform != null)
                lipsSprite = lipsTransform.GetComponent<SpriteRenderer>();
        }

        SetAcneVisible(true);

        if (eyeshadowSprite != null)
            eyeshadowSprite.enabled = false;

        if (lipsSprite != null)
            lipsSprite.enabled = false;
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
        if (eyeshadowSprite == null) return;
        if (shadowSprite == null) return;

        eyeshadowSprite.sprite = shadowSprite;
        eyeshadowSprite.enabled = true;
        hasShadow = true;
        Debug.Log("Shadow applied");
    }

    public void ApplyLipstick(Sprite lipstickSprite)
    {
        if (lipsSprite == null) return;

        if (lipstickSprite != null)
            lipsSprite.sprite = lipstickSprite;

        lipsSprite.enabled = true;
        hasLipstick = true;
        Debug.Log("Lipstick applied");
    }

    // Метод для стирания всего макияжа
    public void ClearAllMakeup()
    {
        // Возвращаем прыщи
        SetAcneVisible(true);
        hasAcne = true;

        // Выключаем тени
        if (eyeshadowSprite != null)
            eyeshadowSprite.enabled = false;
        hasShadow = false;

        // Выключаем помаду
        if (lipsSprite != null)
            lipsSprite.enabled = false;
        hasLipstick = false;

        Debug.Log("All makeup cleared by sponge");
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

    public void ClearLipstick()
    {
        if (hasLipstick)
        {
            if (lipsSprite != null)
                lipsSprite.enabled = false;
            hasLipstick = false;
            Debug.Log("Lipstick cleared");
        }
    }

    private void SetAcneVisible(bool visible)
    {
        if (acneSprite != null)
            acneSprite.enabled = visible;
    }

    public bool HasAcne() => hasAcne;
    public bool HasShadow() => hasShadow;
    public bool HasLipstick() => hasLipstick;
}
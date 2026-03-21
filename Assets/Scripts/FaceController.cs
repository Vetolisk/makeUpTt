using UnityEngine;

public class FaceController : MonoBehaviour
{
    [Header("Sprite Renderers")]
    [SerializeField] private SpriteRenderer characterSprite;
    [SerializeField] private SpriteRenderer acneSprite;
    [SerializeField] private SpriteRenderer eyeshadowSprite;
    [SerializeField] private SpriteRenderer lipsSprite;  // добавили губы

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
            else
                Debug.LogWarning("Lips object not found on Character!");
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

        eyeshadowSprite.sprite = shadowSprite;
        eyeshadowSprite.enabled = true;
        hasShadow = true;
        Debug.Log("Shadow applied");
    }

    // НОВЫЙ МЕТОД для помады
    public void ApplyLipstick(Sprite lipstickSprite)
    {
        Debug.Log("=== ApplyLipstick START ===");

        if (lipsSprite == null)
        {
            Debug.LogError("lipsSprite is NULL! Create a child object named 'Lips' on Character.");
            return;
        }

        if (lipstickSprite == null)
        {
            Debug.LogError("lipstickSprite is NULL!");
            return;
        }

        lipsSprite.sprite = lipstickSprite;
        lipsSprite.enabled = true;
        hasLipstick = true;

        Debug.Log("Lipstick applied. Sprite: " + lipstickSprite.name);
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

    public void ClearAllMakeup()
    {
        SetAcneVisible(true);

        if (eyeshadowSprite != null)
            eyeshadowSprite.enabled = false;

        if (lipsSprite != null)
            lipsSprite.enabled = false;

        hasAcne = true;
        hasShadow = false;
        hasLipstick = false;
        Debug.Log("All makeup cleared");
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
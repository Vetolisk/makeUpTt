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
        }

        SetAcneVisible(true);
        SetEyeshadowVisible(false);
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
        if (eyeshadowSprite != null && shadowSprite != null)
        {
            eyeshadowSprite.sprite = shadowSprite;
            SetEyeshadowVisible(true);
            hasShadow = true;
            Debug.Log("Shadow applied");
        }
    }

    public void ClearShadow()
    {
        if (hasShadow)
        {
            SetEyeshadowVisible(false);
            hasShadow = false;
            Debug.Log("Shadow cleared");
        }
    }

    public void ClearAllMakeup()
    {
        SetAcneVisible(true);
        SetEyeshadowVisible(false);
        hasAcne = true;
        hasShadow = false;
        Debug.Log("All makeup cleared");
    }

    private void SetAcneVisible(bool visible)
    {
        if (acneSprite != null)
            acneSprite.enabled = visible;
    }

    private void SetEyeshadowVisible(bool visible)
    {
        if (eyeshadowSprite != null)
            eyeshadowSprite.enabled = visible;
    }

    public bool HasAcne() => hasAcne;
    public bool HasShadow() => hasShadow;
}
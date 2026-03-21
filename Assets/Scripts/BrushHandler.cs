using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BrushHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer handSprite;
    [SerializeField] private Sprite defaultHand;
    [SerializeField] private SpriteRenderer brushTip;

    [Header("Zones")]
    [SerializeField] private Collider2D faceZone;
    [SerializeField] private FaceController faceController;

    [Header("Objects")]
    [SerializeField] private Transform brushObject;
    [SerializeField] private Transform brushInHand;

    private Vector3 startPosition;
    private Vector3 brushStartPosition;
    private bool holdingBrush = false;
    private bool isDragging = false;
    private Color currentColor = Color.white;
    private Sprite currentShadowSprite;
    private string currentColorName = "";

    void Start()
    {
        startPosition = transform.position;

        if (brushObject != null)
            brushStartPosition = brushObject.position;

        if (handSprite == null)
            handSprite = GetComponent<SpriteRenderer>();

        SetHandSprite(defaultHand);

        if (brushInHand == null)
        {
            GameObject anchor = new GameObject("BrushInHand");
            anchor.transform.SetParent(transform);
            anchor.transform.localPosition = new Vector3(0.6f, -0.1f, 0);
            brushInHand = anchor.transform;
        }

        if (brushTip == null && brushInHand != null)
        {
            GameObject tip = new GameObject("BrushTip");
            tip.transform.SetParent(brushInHand);
            tip.transform.localPosition = new Vector3(0.4f, 0, 0);
            brushTip = tip.AddComponent<SpriteRenderer>();
            brushTip.color = Color.white;
            brushTip.sprite = CreateCircleSprite();
        }
    }

    private Sprite CreateCircleSprite()
    {
        Texture2D texture = new Texture2D(32, 32);
        Color[] colors = new Color[32 * 32];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }

        texture.SetPixels(colors);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f));
    }

    public bool IsHoldingBrush() => holdingBrush;

    public void StartDrag()
    {
        isDragging = true;
    }

    public void EndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        isDragging = false;

        if (faceZone != null)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
            if (faceZone.OverlapPoint(worldPoint))
            {
                StartCoroutine(ApplyShadowAndReturn());
                return;
            }
        }

        StartCoroutine(ReturnBrushToStart());
    }

    public void PickColor(Color color, Sprite shadowSprite, string colorName, Vector3 colorPosition)
    {
        currentColor = color;
        currentShadowSprite = shadowSprite;
        currentColorName = colorName;
        StartCoroutine(PickColorAnimation(colorPosition));
    }

    private IEnumerator PickColorAnimation(Vector3 colorPosition)
    {
        holdingBrush = true;

        yield return StartCoroutine(MoveToPosition(transform.position, colorPosition, 0.2f, null));

        yield return StartCoroutine(ScaleAnimation(0.8f, 0.05f));
        yield return StartCoroutine(ScaleAnimation(1f, 0.05f));

        if (brushTip != null)
            brushTip.color = currentColor;

        if (brushObject != null)
        {
            brushObject.SetParent(transform);
            brushObject.localPosition = brushInHand.localPosition;
            brushObject.localScale = Vector3.one * 0.7f;

            Collider2D brushCollider = brushObject.GetComponent<Collider2D>();
            if (brushCollider != null)
                brushCollider.enabled = false;
        }

        Vector3 middlePos = GetMiddlePosition();
        yield return StartCoroutine(MoveToPosition(transform.position, middlePos, 0.2f, null));

        Debug.Log("Brush ready with color: " + currentColorName);
    }

    private IEnumerator ApplyShadowAndReturn()
    {
        Vector3 originalPos = transform.position;
        Vector3 forwardPos = originalPos + new Vector3(0, 0.3f, 0);

        yield return StartCoroutine(MoveToPosition(originalPos, forwardPos, 0.1f, null));
        yield return StartCoroutine(MoveToPosition(forwardPos, originalPos, 0.1f, null));

        yield return StartCoroutine(ScaleAnimation(1.2f, 0.1f));
        yield return StartCoroutine(ScaleAnimation(1f, 0.1f));

        if (faceController != null && currentShadowSprite != null)
            faceController.ApplyShadow(currentShadowSprite);

        yield return StartCoroutine(ReturnBrushToStart());

        Debug.Log("Shadow applied: " + currentColorName);
    }

    private IEnumerator ReturnBrushToStart()
    {
        holdingBrush = false;

        if (brushObject != null)
        {
            brushObject.SetParent(null);

            Collider2D brushCollider = brushObject.GetComponent<Collider2D>();
            if (brushCollider != null)
                brushCollider.enabled = true;

            yield return StartCoroutine(MoveBrushToPosition(brushObject.position, brushStartPosition, 0.3f, null));
            brushObject.localScale = Vector3.one;
        }

        yield return StartCoroutine(MoveToPosition(transform.position, startPosition, 0.3f, null));

        if (brushTip != null)
            brushTip.color = Color.white;

        SetHandSprite(defaultHand);

        currentShadowSprite = null;
        currentColorName = "";
    }

    private IEnumerator MoveToPosition(Vector3 from, Vector3 to, float duration, System.Action onComplete)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
        transform.position = to;
        onComplete?.Invoke();
    }

    private IEnumerator MoveBrushToPosition(Vector3 from, Vector3 to, float duration, System.Action onComplete)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            brushObject.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
        brushObject.position = to;
        onComplete?.Invoke();
    }

    private IEnumerator ScaleAnimation(float targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.one * targetScale;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        transform.localScale = endScale;
    }

    private Vector3 GetMiddlePosition()
    {
        float x = (brushStartPosition.x + 0) / 2 + 0.5f;
        float y = (brushStartPosition.y + 0) / 2 + 0.5f;
        return new Vector3(x, y, 0);
    }

    private void SetHandSprite(Sprite sprite)
    {
        if (handSprite != null && sprite != null)
            handSprite.sprite = sprite;
    }
}
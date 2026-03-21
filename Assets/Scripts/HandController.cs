using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HandController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer handSprite;
    [SerializeField] private Sprite defaultHand;

    [Header("Zones")]
    [SerializeField] private Collider2D faceZone;
    [SerializeField] private FaceController faceController;

    [Header("Cream Objects")]
    [SerializeField] private Transform creamObject;

    [Header("Brush Objects")]
    [SerializeField] private Transform brushObject;
    [SerializeField] private Transform brushInHand;
    [SerializeField] private SpriteRenderer brushTip;

    [Header("Lipstick Settings")]
    [SerializeField] private Transform lipstickInHand;

    private Vector3 startPosition;
    private Vector3 creamStartPosition;
    private Vector3 brushStartPosition;

    private bool holdingCream = false;
    private bool holdingBrush = false;
    private bool holdingLipstick = false;
    private bool isDragging = false;

    private Color currentBrushColor = Color.white;
    private Sprite currentShadowSprite;
    private Sprite currentLipstickSprite;
    private Transform currentLipstickObject;
    private Vector3 currentLipstickStartPosition;

    void Start()
    {
        startPosition = transform.position;

        if (creamObject != null)
            creamStartPosition = creamObject.position;

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

        if (lipstickInHand == null)
        {
            GameObject anchor = new GameObject("LipstickInHand");
            anchor.transform.SetParent(transform);
            anchor.transform.localPosition = new Vector3(-0.38f, 0.737f, 0);
            lipstickInHand = anchor.transform;
        }

        if (brushTip == null && brushInHand != null)
        {
            GameObject tip = new GameObject("BrushTip");
            tip.transform.SetParent(brushInHand);
            tip.transform.localPosition = new Vector3(0.4f, 0, 0);
            brushTip = tip.AddComponent<SpriteRenderer>();
            brushTip.color = Color.white;
        }
    }

    public bool IsHoldingCream() => holdingCream;
    public bool IsHoldingBrush() => holdingBrush;
    public bool IsHoldingLipstick() => holdingLipstick;

    // Проверка, занята ли рука
    public bool IsHandBusy() => holdingCream || holdingBrush || holdingLipstick;

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
                if (holdingCream)
                    StartCoroutine(ApplyCreamAndReturn());
                else if (holdingBrush)
                    StartCoroutine(ApplyShadowAndReturn());
                else if (holdingLipstick)
                    StartCoroutine(ApplyLipstickAndReturn());
                return;
            }
        }

        if (holdingCream)
            StartCoroutine(ReturnCreamToStart());
        else if (holdingBrush)
            StartCoroutine(ReturnBrushToStart());
        else if (holdingLipstick)
            StartCoroutine(ReturnLipstickToStart());
    }

    // ==================== CREAM METHODS ====================
    public void TakeCream()
    {
        // Проверяем, не занята ли рука
        if (IsHandBusy())
        {
            Debug.Log("Hand is busy with another item!");
            return;
        }

        StartCoroutine(TakeCreamAnimation());
    }

    private IEnumerator TakeCreamAnimation()
    {
        yield return StartCoroutine(MoveToPosition(transform.position, creamStartPosition, 0.2f, null));

        yield return StartCoroutine(ScaleAnimation(0.8f, 0.05f));
        yield return StartCoroutine(ScaleAnimation(1f, 0.05f));

        holdingCream = true;

        if (creamObject != null)
        {
            creamObject.SetParent(transform);
            creamObject.localPosition = new Vector3(-0.328f, 0.651f, 0);
            creamObject.localScale = Vector3.one * 0.8f;

            Collider2D creamCollider = creamObject.GetComponent<Collider2D>();
            if (creamCollider != null)
                creamCollider.enabled = false;
        }

        Vector3 middlePos = GetMiddlePosition(creamStartPosition);
        yield return StartCoroutine(MoveToPosition(transform.position, middlePos, 0.2f, null));

        Debug.Log("Cream taken");
    }

    private IEnumerator ApplyCreamAndReturn()
    {
        Vector3 originalPos = transform.position;
        Vector3 forwardPos = originalPos + new Vector3(0, 0.3f, 0);

        yield return StartCoroutine(MoveToPosition(originalPos, forwardPos, 0.1f, null));
        yield return StartCoroutine(MoveToPosition(forwardPos, originalPos, 0.1f, null));

        yield return StartCoroutine(ScaleAnimation(1.2f, 0.1f));
        yield return StartCoroutine(ScaleAnimation(1f, 0.1f));

        if (faceController != null)
            faceController.RemoveAcne();

        yield return StartCoroutine(ReturnCreamToStart());

        Debug.Log("Cream applied");
    }

    private IEnumerator ReturnCreamToStart()
    {
        holdingCream = false;

        if (creamObject != null)
        {
            creamObject.SetParent(null);

            Collider2D creamCollider = creamObject.GetComponent<Collider2D>();
            if (creamCollider != null)
                creamCollider.enabled = true;

            yield return StartCoroutine(MoveObjectToPosition(creamObject, creamObject.position, creamStartPosition, 0.3f, null));
            creamObject.localScale = Vector3.one;
        }

        yield return StartCoroutine(MoveToPosition(transform.position, startPosition, 0.3f, null));

        SetHandSprite(defaultHand);
    }

    // ==================== BRUSH METHODS ====================
    public void PickColor(Color color, Sprite shadowSprite, Vector3 colorPosition)
    {
        // Проверяем, не занята ли рука
        if (IsHandBusy())
        {
            Debug.Log("Hand is busy with another item!");
            return;
        }

        currentBrushColor = color;
        currentShadowSprite = shadowSprite;
        StartCoroutine(PickColorAnimation(colorPosition));
    }

    private IEnumerator PickColorAnimation(Vector3 colorPosition)
    {
        yield return StartCoroutine(MoveToPosition(transform.position, colorPosition, 0.2f, null));

        yield return StartCoroutine(ScaleAnimation(0.8f, 0.05f));
        yield return StartCoroutine(ScaleAnimation(1f, 0.05f));

        if (brushTip != null)
            brushTip.color = currentBrushColor;

        holdingBrush = true;

        if (brushObject != null)
        {
            brushObject.SetParent(transform);
            brushObject.localPosition = brushInHand.localPosition;
            brushObject.localScale = Vector3.one * 0.7f;

            Collider2D brushCollider = brushObject.GetComponent<Collider2D>();
            if (brushCollider != null)
                brushCollider.enabled = false;
        }

        Vector3 middlePos = GetMiddlePosition(brushStartPosition);
        yield return StartCoroutine(MoveToPosition(transform.position, middlePos, 0.2f, null));

        Debug.Log("Brush ready");
    }

    private IEnumerator ApplyShadowAndReturn()
    {
        Vector3 originalPos = transform.position;

        Vector3 rightPos = originalPos + new Vector3(0.35f, 0, 0);
        Vector3 leftPos = originalPos + new Vector3(-0.35f, 0, 0);

        yield return StartCoroutine(MoveToPosition(originalPos, rightPos, 0.15f, null));
        yield return StartCoroutine(MoveToPosition(rightPos, leftPos, 0.3f, null));
        yield return StartCoroutine(MoveToPosition(leftPos, rightPos, 0.3f, null));
        yield return StartCoroutine(MoveToPosition(rightPos, originalPos, 0.15f, null));

        yield return StartCoroutine(ScaleAnimation(1.15f, 0.12f));
        yield return StartCoroutine(ScaleAnimation(1f, 0.12f));

        if (faceController != null && currentShadowSprite != null)
            faceController.ApplyShadow(currentShadowSprite);

        yield return StartCoroutine(ReturnBrushToStart());

        Debug.Log("Shadow applied");
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

            yield return StartCoroutine(MoveObjectToPosition(brushObject, brushObject.position, brushStartPosition, 0.3f, null));
            brushObject.localScale = Vector3.one;
        }

        yield return StartCoroutine(MoveToPosition(transform.position, startPosition, 0.3f, null));

        if (brushTip != null)
            brushTip.color = Color.white;

        SetHandSprite(defaultHand);

        currentShadowSprite = null;
    }

    // ==================== LIPSTICK METHODS ====================
    public void TakeLipstick(Transform lipstickObject, Sprite lipstickSprite = null)
    {
        // Проверяем, не занята ли рука
        if (IsHandBusy())
        {
            Debug.Log("Hand is busy with another item!");
            return;
        }

        currentLipstickObject = lipstickObject;
        currentLipstickStartPosition = lipstickObject.position;
        currentLipstickSprite = lipstickSprite;
        StartCoroutine(TakeLipstickAnimation());
    }

    private IEnumerator TakeLipstickAnimation()
    {
        yield return StartCoroutine(MoveToPosition(transform.position, currentLipstickStartPosition, 0.2f, null));

        yield return StartCoroutine(ScaleAnimation(0.8f, 0.05f));
        yield return StartCoroutine(ScaleAnimation(1f, 0.05f));

        holdingLipstick = true;

        if (currentLipstickObject != null)
        {
            currentLipstickObject.SetParent(transform);

            if (lipstickInHand != null)
                currentLipstickObject.localPosition = lipstickInHand.localPosition;
            else
                currentLipstickObject.localPosition = new Vector3(0.5f, -0.2f, 0);

            currentLipstickObject.localScale = Vector3.one * 0.8f;

            Collider2D lipstickCollider = currentLipstickObject.GetComponent<Collider2D>();
            if (lipstickCollider != null)
                lipstickCollider.enabled = false;
        }

        Vector3 middlePos = GetMiddlePosition(currentLipstickStartPosition);
        yield return StartCoroutine(MoveToPosition(transform.position, middlePos, 0.2f, null));

        Debug.Log("Lipstick taken");
    }

    private IEnumerator ApplyLipstickAndReturn()
    {
        Vector3 originalPos = transform.position;
        Vector3 forwardPos = originalPos + new Vector3(0, 0.3f, 0);

        yield return StartCoroutine(MoveToPosition(originalPos, forwardPos, 0.1f, null));
        yield return StartCoroutine(MoveToPosition(forwardPos, originalPos, 0.1f, null));

        yield return StartCoroutine(ScaleAnimation(1.2f, 0.1f));
        yield return StartCoroutine(ScaleAnimation(1f, 0.1f));

        if (faceController != null)
            faceController.ApplyLipstick(currentLipstickSprite);

        yield return StartCoroutine(ReturnLipstickToStart());

        Debug.Log("Lipstick applied");
    }

    private IEnumerator ReturnLipstickToStart()
    {
        holdingLipstick = false;

        if (currentLipstickObject != null)
        {
            currentLipstickObject.SetParent(null);

            Collider2D lipstickCollider = currentLipstickObject.GetComponent<Collider2D>();
            if (lipstickCollider != null)
                lipstickCollider.enabled = true;

            yield return StartCoroutine(MoveObjectToPosition(currentLipstickObject, currentLipstickObject.position, currentLipstickStartPosition, 0.3f, null));
            currentLipstickObject.localScale = Vector3.one;

            currentLipstickObject = null;
        }

        yield return StartCoroutine(MoveToPosition(transform.position, startPosition, 0.3f, null));

        SetHandSprite(defaultHand);

        currentLipstickSprite = null;
    }

    // ==================== UTILITY METHODS ====================
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

    private IEnumerator MoveObjectToPosition(Transform obj, Vector3 from, Vector3 to, float duration, System.Action onComplete)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            obj.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
        obj.position = to;
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

    private Vector3 GetMiddlePosition(Vector3 itemStartPosition)
    {
        float x = (itemStartPosition.x + 0) / 2 + 0.5f;
        float y = (itemStartPosition.y + 0) / 2 + 0.5f;
        return new Vector3(x, y, 0);
    }

    private void SetHandSprite(Sprite sprite)
    {
        if (handSprite != null && sprite != null)
            handSprite.sprite = sprite;
    }

    public void ResetToIdle()
    {
        StopAllCoroutines();

        if (holdingCream)
            StartCoroutine(ReturnCreamToStart());
        else if (holdingBrush)
            StartCoroutine(ReturnBrushToStart());
        else if (holdingLipstick)
            StartCoroutine(ReturnLipstickToStart());
    }
}
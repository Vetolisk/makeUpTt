using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HandController : MonoBehaviour
{
    [Header("Компоненты")]
    [SerializeField] private SpriteRenderer handSprite;
    [SerializeField] private Sprite defaultHand;

    [Header("Зоны")]
    [SerializeField] private Collider2D faceZone;
    [SerializeField] private FaceController faceController;

    [Header("Объекты")]
    [SerializeField] private Transform creamObject;

    private Vector3 startPosition;
    private Vector3 creamStartPosition;
    private bool holdingCream = false;
    private bool isDragging = false;

    void Start()
    {
        startPosition = transform.position;

        if (creamObject != null)
            creamStartPosition = creamObject.position;

        if (handSprite == null)
            handSprite = GetComponent<SpriteRenderer>();

        SetHandSprite(defaultHand);
    }

    public bool IsHoldingCream() => holdingCream;

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
                StartCoroutine(ApplyCreamAndReturn());
                return;
            }
        }

        StartCoroutine(ReturnCreamToStart());
    }

    public void TakeCream()
    {
        StartCoroutine(TakeCreamAnimation());
    }

    private IEnumerator TakeCreamAnimation()
    {
        // 1. Двигаем руку к крему
        yield return StartCoroutine(MoveToPosition(transform.position, creamStartPosition, 0.2f, null));

        // 2. Анимация "схватить"
        yield return StartCoroutine(ScaleAnimation(0.8f, 0.05f));
        yield return StartCoroutine(ScaleAnimation(1f, 0.05f));

        // 3. Крем "магнитится" к руке
        holdingCream = true;

        if (creamObject != null)
        {
            // Делаем крем дочерним объектом руки
            creamObject.SetParent(transform);
            // Сохраняем позицию крема относительно руки (чтобы он был в руке)
            creamObject.localPosition = new Vector3(-0.305f, 0.568f, 0);
            // Уменьшаем немного для реализма
            creamObject.localScale = Vector3.one * 0.8f;

            // Отключаем коллайдер у крема, чтобы он не мешал перетаскиванию
            Collider2D creamCollider = creamObject.GetComponent<Collider2D>();
            if (creamCollider != null)
                creamCollider.enabled = false;
        }

        // 4. Двигаем руку в промежуточную позицию
        Vector3 middlePos = GetMiddlePosition();
        yield return StartCoroutine(MoveToPosition(transform.position, middlePos, 0.2f, null));

        Debug.Log("✅ Крем взят! Теперь перетащи руку к лицу");
    }

    private IEnumerator ApplyCreamAndReturn()
    {
        // 1. Анимация нанесения
        Vector3 originalPos = transform.position;
        Vector3 forwardPos = originalPos + new Vector3(0, 0.3f, 0);

        yield return StartCoroutine(MoveToPosition(originalPos, forwardPos, 0.1f, null));
        yield return StartCoroutine(MoveToPosition(forwardPos, originalPos, 0.1f, null));

        // 2. Анимация масштаба
        yield return StartCoroutine(ScaleAnimation(1.2f, 0.1f));
        yield return StartCoroutine(ScaleAnimation(1f, 0.1f));

        // 3. Убираем прыщи
        if (faceController != null)
            faceController.RemoveAcne();

        // 4. Возвращаем крем на место
        yield return StartCoroutine(ReturnCreamToStart());

        Debug.Log("✅ Крем нанесён!");
    }

    private IEnumerator ReturnCreamToStart()
    {
        holdingCream = false;

        // Открепляем крем от руки
        if (creamObject != null)
        {
            creamObject.SetParent(null);

            // Включаем коллайдер обратно
            Collider2D creamCollider = creamObject.GetComponent<Collider2D>();
            if (creamCollider != null)
                creamCollider.enabled = true;

            // Возвращаем крем на исходную позицию
            yield return StartCoroutine(MoveCreamToPosition(creamObject.position, creamStartPosition, 0.3f, null));

            // Возвращаем исходный масштаб
            creamObject.localScale = Vector3.one;
        }

        // Возвращаем руку
        yield return StartCoroutine(MoveToPosition(transform.position, startPosition, 0.3f, null));

        SetHandSprite(defaultHand);
    }

    private IEnumerator MoveCreamToPosition(Vector3 from, Vector3 to, float duration, System.Action onComplete)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            creamObject.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
        creamObject.position = to;
        onComplete?.Invoke();
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
        float x = (creamStartPosition.x + 0) / 2 + 0.5f;
        float y = (creamStartPosition.y + 0) / 2 + 0.5f;
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
        StartCoroutine(ReturnCreamToStart());
    }
}
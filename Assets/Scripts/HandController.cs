using UnityEngine;
using UnityEngine.EventSystems;

public class HandController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("References")]
    [SerializeField] private RectTransform handRect;
    [SerializeField] private ZoneDetector faceZone;
    [SerializeField] private Canvas mainCanvas;

    [Header("Settings")]
    [SerializeField] private float dragSpeed = 1f;
    [SerializeField] private float returnDuration = 0.3f;
    [SerializeField] private float animationDuration = 0.2f;

    public enum HandState
    {
        Idle,
        HoldingCream,
        HoldingBrush,
        HoldingLipstick,
        Returning
    }

    private HandState currentState = HandState.Idle;
    private Vector2 startPosition;
    private bool isDragging = false;
    private Vector2 dragOffset;

    // События
    public System.Action OnMakeupApplied;
    public System.Action OnMakeupCancelled;
    public System.Action<HandState> OnStateChanged;

    private void Start()
    {
        if (handRect == null) handRect = GetComponent<RectTransform>();
        startPosition = handRect.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentState == HandState.Idle || currentState == HandState.Returning) return;

        isDragging = true;
        dragOffset = handRect.anchoredPosition - eventData.position;
        GameManager.Instance?.SetState(GameManager.GameState.ApplyingMakeup);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        handRect.anchoredPosition = eventData.position + dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        isDragging = false;

        if (faceZone != null && faceZone.IsPointerInZone(eventData))
        {
            OnReleaseInFaceZone();
        }
        else
        {
            OnReleaseOutsideZone();
        }
    }

    private void OnReleaseInFaceZone()
    {
        Debug.Log("Released in face zone - applying makeup");

        AnimateApply(() =>
        {
            OnMakeupApplied?.Invoke();
            ReturnToStartPosition();
        });
    }

    private void OnReleaseOutsideZone()
    {
        Debug.Log("Released outside face zone - cancelling");
        OnMakeupCancelled?.Invoke();
        ReturnToStartPosition();
    }

    private void AnimateApply(System.Action onComplete)
    {
        GameManager.Instance?.SetState(GameManager.GameState.ApplyingMakeup);

        LeanTween.scale(handRect, Vector3.one * 1.2f, animationDuration * 0.5f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                LeanTween.scale(handRect, Vector3.one, animationDuration * 0.5f)
                    .setOnComplete(() => onComplete?.Invoke());
            });
    }

    private void ReturnToStartPosition()
    {
        GameManager.Instance?.SetState(GameManager.GameState.Returning);
        SetState(HandState.Returning);

        LeanTween.move(handRect, startPosition, returnDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                SetState(HandState.Idle);
                GameManager.Instance?.SetState(GameManager.GameState.Playing);
            });
    }

    /// <summary>
    /// Анимация взятия предмета рукой
    /// </summary>
    public void AnimateTakeItem(Vector2 itemPosition, System.Action onComplete = null)
    {
        // Плавно двигаем руку к предмету
        LeanTween.move(handRect, itemPosition, 0.2f)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() =>
            {
                // Анимация "взятия" предмета
                LeanTween.scale(handRect, Vector3.one * 0.9f, 0.05f)
                    .setEase(LeanTweenType.easeOutQuad)
                    .setOnComplete(() =>
                    {
                        LeanTween.scale(handRect, Vector3.one, 0.05f)
                            .setOnComplete(() =>
                            {
                                onComplete?.Invoke();
                            });
                    });
            });
    }

    /// <summary>
    /// Переместить руку в указанную позицию
    /// </summary>
    public void MoveToPosition(Vector2 position, System.Action onComplete = null)
    {
        LeanTween.move(handRect, position, animationDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setOnComplete(() => onComplete?.Invoke());
    }

    public void SetState(HandState newState)
    {
        currentState = newState;
        OnStateChanged?.Invoke(currentState);
        Debug.Log($"Hand state changed to: {currentState}");
    }

    public void ResetToIdle()
    {
        LeanTween.cancel(handRect.gameObject);
        handRect.anchoredPosition = startPosition;
        handRect.localScale = Vector3.one;
        SetState(HandState.Idle);
    }

    public HandState GetCurrentState() => currentState;
}
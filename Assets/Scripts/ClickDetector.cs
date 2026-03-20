using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    [SerializeField] private HandController handController;

    void Start()
    {
        if (handController == null)
            handController = FindObjectOfType<HandController>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                Vector2 touchPos2D = new Vector2(touchPos.x, touchPos.y);

                RaycastHit2D hit = Physics2D.Raycast(touchPos2D, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject.CompareTag("Cream"))
                {
                    Debug.Log("Cream tapped");

                    if (handController != null && !handController.IsHoldingCream())
                    {
                        handController.TakeCream();
                    }
                }
            }
        }
    }
}
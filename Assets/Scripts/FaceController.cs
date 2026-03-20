using UnityEngine;

public class FaceController : MonoBehaviour
{
    [Header("Face Parts")]
    [SerializeField] private SpriteRenderer faceRenderer;
    [SerializeField] private SpriteRenderer eyesRenderer;
    [SerializeField] private SpriteRenderer lipsRenderer;

    [Header("Sprites")]
    [SerializeField] private Sprite faceWithAcne;
    [SerializeField] private Sprite faceWithoutAcne;
    [SerializeField] private Sprite eyesWithoutShadow;
    [SerializeField] private Sprite eyesWithShadow;
    [SerializeField] private Sprite lipsWithoutLipstick;
    [SerializeField] private Sprite lipsWithLipstick;

    private bool hasMakeup = false;

    public void ApplyCream()
    {
        if (faceRenderer != null && faceWithAcne != null && faceWithoutAcne != null)
        {
            faceRenderer.sprite = faceWithoutAcne;
            Debug.Log("Cream applied - acne removed!");
        }
    }

    public void ApplyShadow()
    {
        if (eyesRenderer != null && eyesWithoutShadow != null && eyesWithShadow != null)
        {
            eyesRenderer.sprite = eyesWithShadow;
            Debug.Log("Shadow applied!");
        }
    }

    public void ApplyLipstick()
    {
        if (lipsRenderer != null && lipsWithoutLipstick != null && lipsWithLipstick != null)
        {
            lipsRenderer.sprite = lipsWithLipstick;
            Debug.Log("Lipstick applied!");
        }
    }

    public void ClearMakeup()
    {
        if (faceRenderer != null && faceWithAcne != null)
            faceRenderer.sprite = faceWithAcne;

        if (eyesRenderer != null && eyesWithoutShadow != null)
            eyesRenderer.sprite = eyesWithoutShadow;

        if (lipsRenderer != null && lipsWithoutLipstick != null)
            lipsRenderer.sprite = lipsWithoutLipstick;

        hasMakeup = false;
        Debug.Log("All makeup cleared!");
    }
}
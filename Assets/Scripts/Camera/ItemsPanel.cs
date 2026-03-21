using UnityEngine;

public class ItemsPanel : MonoBehaviour
{
    [SerializeField] private ScreenAnchor.AnchorPosition panelAnchor = ScreenAnchor.AnchorPosition.BottomCenter;
    [SerializeField] private Vector2 panelOffset = new Vector2(0, 150);
    [SerializeField] private float itemSpacing = 100f;
    [SerializeField] private Transform[] items;

    void Start()
    {
        PositionItems();
    }

    void PositionItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            float xOffset = (i - (items.Length - 1) / 2f) * itemSpacing;
            items[i].localPosition = new Vector3(xOffset, 0, 0);
        }
    }
}
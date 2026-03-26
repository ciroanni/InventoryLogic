using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDragController
{
    // classe di supporto per gestire l'icona che segue il mouse durante il drag and drop degli oggetti nell'inventario
    private readonly GameObject _root;
    private readonly RectTransform _rectTransform;
    private readonly Canvas _canvas;

    public InventoryDragController(Canvas canvas, Sprite icon)
    {
        _canvas = canvas;

        _root = new GameObject("InventoryDragController");
        _root.transform.SetParent(canvas.transform, false);

        _rectTransform = _root.AddComponent<RectTransform>();
        _rectTransform.sizeDelta = new Vector2(52f, 52f); // dimensione rimpicciolita dell'icona per il drag

        Image image = _root.AddComponent<Image>();
        image.raycastTarget = false;
        image.sprite = icon;
        image.preserveAspect = true;
        image.color = new Color(1f, 1f, 1f, 0.85f);
    }

    public void FollowPointer(PointerEventData eventData)
    {
        if (_canvas == null)
        {
            return;
        }

        RectTransform canvasRect = _canvas.transform as RectTransform;
        if (canvasRect == null)
        {
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPos);

        _rectTransform.localPosition = localPos;
    }

    public void Dispose()
    {
        if (_root != null)
        {
            Object.Destroy(_root);
        }
    }
}

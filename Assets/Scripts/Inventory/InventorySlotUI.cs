using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    [Header("UI")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI quantityText;

    private int _slotIndex;
    private InventoryUI _inventoryUI;
    private Canvas _rootCanvas;

    private bool _hasItem;

    private static InventorySlotUI dragSource;
    private static InventoryDragController dragController;

    public void Initialize(int slotIndex, InventoryUI inventoryUI, Canvas rootCanvas)
    {
        _slotIndex = slotIndex;
        _inventoryUI = inventoryUI;
        _rootCanvas = rootCanvas;
    }

    public void SetData(InventorySlotInfo slotInfo)
    {
        _hasItem = slotInfo != null && !slotInfo.isEmpty;

        if (!_hasItem)
        {
            if (iconImage != null)
            {
                iconImage.enabled = false;
                iconImage.sprite = null;
            }

            if (quantityText != null)
            {
                quantityText.text = string.Empty;
            }
            return;
        }

        if (iconImage != null)
        {
            iconImage.enabled = true;
            iconImage.sprite = slotInfo.item.Icon;
        }

        if (quantityText != null)
        {
            quantityText.text = slotInfo.quantity > 1 ? slotInfo.quantity.ToString() : string.Empty;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_hasItem || _rootCanvas == null)
        {
            return;
        }

        // sorgente drag globale per sapere da quale slot parte lo swap.
        dragSource = this;
        dragController = new InventoryDragController(_rootCanvas, iconImage != null ? iconImage.sprite : null);
        dragController.FollowPointer(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragController?.FollowPointer(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragController?.Dispose();
        dragController = null;
        dragSource = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (dragSource == null || dragSource == this)
        {
            return;
        }

        // drop su un altro slot => scambio posizioni inventario.
        _inventoryUI.HandleDrop(dragSource._slotIndex, _slotIndex);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // click destro => usa item nello slot
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            _inventoryUI.HandleUseRequest(_slotIndex);
        }
    }
}

using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private InventorySlotUI slotPrefab;
    [SerializeField] private Transform slotsRoot;
    [SerializeField] private Canvas rootCanvas;
    [SerializeField] private PlayerInput playerInputs;
    [SerializeField] private FollowPlayer followPlayer;

    [Header("Input")]
    [SerializeField] private Key toggleKey = Key.Tab;
    private readonly List<InventorySlotUI> _slotUIs = new List<InventorySlotUI>();
    private bool _isVisible;

    private void Awake()
    {
        if (inventorySystem == null)
        {
            inventorySystem = FindFirstObjectByType<InventorySystem>();
        }

        if (rootCanvas == null)
        {
            rootCanvas = GetComponentInChildren<Canvas>(true);
        }

        if (playerInputs == null)
        {
            playerInputs = FindFirstObjectByType<PlayerInput>();
        }

        if (followPlayer == null)
        {
            followPlayer = FindFirstObjectByType<FollowPlayer>();
        }
    }

    private void Start()
    {
        BuildSlots();
        Refresh();
        SetVisible(false); // all'inizio l'inventario è chiuso
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null && keyboard[toggleKey].wasPressedThisFrame)
        {
            ToggleVisibility();
        }
    }

    private void OnEnable()
    {
        if (inventorySystem != null)
        {
            inventorySystem.OnInventoryChanged += Refresh;
        }

        Refresh();
    }

    private void OnDisable()
    {
        if (inventorySystem != null)
        {
            inventorySystem.OnInventoryChanged -= Refresh;
        }
    }

    private void BuildSlots()
    {
        if (inventorySystem == null || slotPrefab == null || slotsRoot == null)
        {
            return;
        }

        // distruggo eventuali slot già presenti
        _slotUIs.Clear();
        for (int child = slotsRoot.childCount - 1; child >= 0; child--)
        {
            Destroy(slotsRoot.GetChild(child).gameObject);
        }
        

        for (int i = 0; i < inventorySystem.SlotNumbers; i++)
        {
            InventorySlotUI slot = Instantiate(slotPrefab, slotsRoot);
            slot.Initialize(i, this, rootCanvas);
            _slotUIs.Add(slot);
        }
    }

    public void Refresh()
    {
        if (inventorySystem == null || _slotUIs.Count == 0)
        {
            return;
        }
        // refresh di tutti gli slot, ad esempio dopo uno swap o un uso 
        for (int i = 0; i < _slotUIs.Count; i++)
        {
            InventorySlotInfo slotInfo = inventorySystem.GetSlots()[i];
            InventorySlotUI slotUI = _slotUIs[i];
            slotUI.SetData(slotInfo);
        }
    }

    public void HandleDrop(int fromIndex, int toIndex)
    {
        if (inventorySystem == null)
        {
            return;
        }

        inventorySystem.SwapSlots(fromIndex, toIndex);
    }

    public void HandleUseRequest(int slotIndex)
    {
        if (inventorySystem == null)
        {
            return;
        }

        inventorySystem.UseItem(slotIndex);
    }

    public void ToggleVisibility()
    {
        SetVisible(!_isVisible);
    }

    public void SetVisible(bool isVisible)
    {
        _isVisible = isVisible;

        if (rootCanvas != null)
        {
            rootCanvas.enabled = _isVisible;
        }

        if (_isVisible)
        {
            Refresh();
        }

        TogglePlayerInputControl();
    }

    private void TogglePlayerInputControl()
    {
        // quando l'inventario è aperto, blocco i comandi del player e mostro il cursore, altrimenti li riabilito e nascondo il cursore
        bool shouldBlockPlayerControl = _isVisible;

        if (playerInputs != null)
        {
            playerInputs.enabled = !shouldBlockPlayerControl;
            followPlayer.enabled = !shouldBlockPlayerControl;
        }

        Cursor.lockState = shouldBlockPlayerControl ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = shouldBlockPlayerControl;
    }
}

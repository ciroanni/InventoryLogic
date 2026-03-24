using System.Collections.Generic;
using UnityEngine;
using System;

public class InventorySystem : MonoBehaviour
{
    [Header("Inventory")]
    [Min(1)]
    [SerializeField] private int slotNumbers = 16;

    private readonly List<InventorySlotInfo> slots = new List<InventorySlotInfo>();

    public event Action OnInventoryChanged;
    public event Action<ItemData, int> OnItemUsed;

    public int SlotNumbers => slotNumbers;

    void Awake()
    {
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        slots.Clear();
        for (int i = 0; i < slotNumbers; i++)
        {
            slots.Add(new InventorySlotInfo());
        }
    }

    public bool AddItem(ItemData item, int quantity = 1)
    {
        if (item == null || quantity <= 0)
        {
            return false;
        }

        if (GetRemainingCapacity(item) < quantity)
        {
            return false;
        }

        int remaining = quantity;

        // prima faccio stack degli item uguali già presenti
        for (int i = 0; i < slots.Count; i++)
        {
            InventorySlotInfo slot = slots[i];

            if (slot.item != item)
                continue;

            int spaceLeft = item.MaxStack - slot.quantity;
            if (spaceLeft <= 0)
                continue;

            int quantityToAdd = Mathf.Min(spaceLeft, remaining);
            slot.quantity += quantityToAdd;
            remaining -= quantityToAdd;

            if (remaining <= 0)
            {
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        // aggiungo item nuovi negli slot vuoti
        for (int i = 0; i < slots.Count; i++)
        {
            InventorySlotInfo slot = slots[i];
            if (slot.isEmpty)
            {
                int quantityToAdd = Mathf.Min(item.MaxStack, remaining);
                slot.item = item;
                slot.quantity = quantityToAdd;
                remaining -= quantityToAdd;

                if (remaining <= 0)
                {
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }
        }

        return remaining <= 0;
    }

    public bool UseItem(int slotIndex)
    {
        if (!IsValidSlot(slotIndex))
        {
            return false;
        }

        InventorySlotInfo slot = slots[slotIndex];
        if (slot.isEmpty)
        {
            return false;
        }

        // use item

        RemoveItem(slotIndex);
        //OnItemUsed?.Invoke(itemToUse, slotIndex);
        return true;
    }

    public bool RemoveItem(int slotIndex, int quantity = 1)
    {
        if (!IsValidSlot(slotIndex) || quantity <= 0)
        {
            return false;
        }

        InventorySlotInfo slot = slots[slotIndex];
        if (slot.isEmpty)
        {
            return false;
        }

        slot.quantity -= quantity;
        if (slot.quantity <= 0)
        {
            slot.Clear();
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool SwapSlots(int slotIndexA, int slotIndexB)
    {
        if (!IsValidSlot(slotIndexA) || !IsValidSlot(slotIndexB) || slotIndexA == slotIndexB)
        {
            return false;
        }

        InventorySlotInfo temp = slots[slotIndexA];
        slots[slotIndexA] = slots[slotIndexB];
        slots[slotIndexB] = temp;

        OnInventoryChanged?.Invoke();

        return true;
    }

    public IReadOnlyList<InventorySlotInfo> GetSlots()
    {
        return slots;
    }

    private int GetRemainingCapacity(ItemData item)
    {
        int capacity = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            InventorySlotInfo slot = slots[i];

            if (slot.isEmpty)
            {
                capacity += item.MaxStack;
                continue;
            }

            if (slot.item == item)
            {
                capacity += Mathf.Max(0, item.MaxStack - slot.quantity);
            }
        }

        return capacity;
    }

    private bool IsValidSlot(int index)
    {
        return index >= 0 && index < slots.Count;
    }

}

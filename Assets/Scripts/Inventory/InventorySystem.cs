using System.Collections.Generic;
using UnityEngine;
using System;

public class InventorySystem : MonoBehaviour
{
    [Header("Inventory")]
    [Min(1)]
    [SerializeField] private int slotNumbers = 18;

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

        // remaining è la quantità di item che ancora devo aggiungere mentre itero sugli slot
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

            // aggiungo quanti item riesco a stackare in questo slot
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

        ItemData itemToUse = slot.item;
        if (itemToUse == null)
        {
            return false;
        }

        // un item può avere più effetti, ad esempio un cibo potrebbe curare HP e aumentare temporaneamente la velocità di movimento
        IReadOnlyList<ItemEffect> effects = itemToUse.Effects; // readonly per sicurezza, non voglio che gli effetti vengano modificati da fuori
        if (effects == null || effects.Count == 0)
        {
            return false;
        }

        // contesto con tutte le info che potrebbero servire agli effetti
        ItemEffectContext context = new ItemEffectContext(gameObject, transform, this, itemToUse, slotIndex); 
        bool appliedAnyEffect = false;

        for (int i = 0; i < effects.Count; i++)
        {
            ItemEffect effect = effects[i];
            if (effect == null)
            {
                continue;
            }

            if (effect.Apply(context))
            {
                appliedAnyEffect = true;
            }
        }

        if (!appliedAnyEffect)
        {
            return false;
        }

        RemoveItem(slotIndex);
        OnItemUsed?.Invoke(itemToUse, slotIndex);
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

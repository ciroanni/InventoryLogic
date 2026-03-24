using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("Inventory")]
    [Min(1)]
    [SerializeField] private int slotNumbers = 16;

    private readonly List<InventorySlotInfo> slots = new List<InventorySlotInfo>();

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
    
    void Start()
    {

    }

    void Update()
    {

    }
}

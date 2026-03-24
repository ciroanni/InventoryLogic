using UnityEngine;

public readonly struct ItemEffectContext
{
    // contesto che viene passato agli item effect quando vengono applicati, contiene tutte le info che potrebbero servire agli effetti per funzionare
    // userObject chi ha usato l'item
    // userTransform è il suo transform (utile per effetti che devono spawnare qualcosa o roba simile)
    // inventory è il suo inventario (utile per effetti che devono modificare l'inventario)
    // item è l'item che è stato usato
    // slotIndex è l'indice dello slot da cui è stato usato l'item (utile per effetti che devono modificare la quantità dell'item o simili)
    public ItemEffectContext(GameObject userObject, Transform userTransform, InventorySystem inventory, ItemData item, int slotIndex)
    {
        UserObject = userObject;
        UserTransform = userTransform;
        Inventory = inventory;
        Item = item;
        SlotIndex = slotIndex;
    }

    public GameObject UserObject { get; }
    public Transform UserTransform { get; }
    public InventorySystem Inventory { get; }
    public ItemData Item { get; }
    public int SlotIndex { get; }
}

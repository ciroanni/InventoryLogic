using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Inventory/Item Data", fileName = "ItemData")]
public class ItemData : ScriptableObject
{
    // uso uno scriptable object per definire i dati di un item, così da poterli modificare facilmente nell'inspector e 
    // creare nuovi item senza dover scrivere codice
    [Header("General")]
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;

    [Header("Effects")]
    [SerializeField] private List<ItemEffect> effects = new List<ItemEffect>();

    [Header("Visual")]
    [SerializeField] private Sprite itemIcon;

    [Header("Inventory")]
    [Min(1)]
    [SerializeField] private int maxStack = 1;

    public string ItemName => itemName;
    public string Description => itemDescription;
    public Sprite Icon => itemIcon;
    public int MaxStack => maxStack;
    public IReadOnlyList<ItemEffect> Effects => effects;
}

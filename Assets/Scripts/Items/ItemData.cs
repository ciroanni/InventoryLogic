using UnityEngine;

public class ItemData : ScriptableObject
{
    // uso uno scriptable object per definire i dati di un item, così da poterli modificare facilmente nell'inspector e 
    // creare nuovi item senza dover scrivere codice
    [Header("General")]
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;

    [Header("Effects")]
    [SerializeField] private ItemEffect itemEffect;
    [Min(0f)]
    [SerializeField] private float effectValue = 1.5f;
    [Min(0f)]
    [SerializeField] private float durationSeconds = 5f;

    [Header("Visual")]
    [SerializeField] private Sprite itemIcon;

    [Header("Inventory")]
    [Min(1)]
    [SerializeField] private int maxStack = 1;

    public string ItemName => itemName;
    public string Description => itemDescription;
    public Sprite Icon => itemIcon;
    public int MaxStack => maxStack;
    public ItemEffect Effect => itemEffect;
    public float EffectValue => effectValue;
    public float DurationSeconds => durationSeconds;
}

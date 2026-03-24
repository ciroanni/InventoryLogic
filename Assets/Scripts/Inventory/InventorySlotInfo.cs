[System.Serializable]
public class InventorySlotInfo
{
    //per tenere traccia di quale item c'è nel singolo slot e quanti ne ho
    //così mi evito controlli sparsi di null e 0 in giro per il codice, e posso semplicemente chiedere se lo slot è vuoto o meno
    public ItemData item;
    public int quantity;
    public bool isEmpty => item == null || quantity <= 0;

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}

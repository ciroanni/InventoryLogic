using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    // enum a pensarci era poco estendibile
    // dovevo ogni volta modificarlo se volevo aggiungere un nuovo item (ok che è un task limitato ma se pensiamo in ottica di un gioco più grande magari è meglio così)
    public abstract bool Apply(ItemEffectContext context);
}

using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Effects/Timed Player Stat", fileName = "TimedPlayerStatEffect")]
public class TimedPlayerStatEffect : ItemEffect
{
    // effetti di item che modificano temporaneamente una stat del player, ad esempio move speed, jump height, ecc.
    [SerializeField] private PlayerStatType statType = PlayerStatType.MoveSpeed;
    [Min(0.01f)]
    [SerializeField] private float multiplier = 1.25f;
    [Min(0.01f)]
    [SerializeField] private float durationSeconds = 5f;

    public override bool Apply(ItemEffectContext context)
    {
        if (context.UserObject == null)
        {
            return false;
        }
        // aggiungo il runtime controller al player se non ce l'ha già, e gli dico di applicare il modificatore temporaneo alla stat specificata
        EffectRuntimeController runtimeController = context.UserObject.GetComponent<EffectRuntimeController>();
        if (runtimeController == null)
        {
            runtimeController = context.UserObject.AddComponent<EffectRuntimeController>();
        }

        return runtimeController.ApplyTimedStatMultiplier(statType, multiplier, durationSeconds);
    }
}

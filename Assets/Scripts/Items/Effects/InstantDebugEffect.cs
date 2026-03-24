using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Effects/Instant Debug Message", fileName = "InstantDebugEffect")]
public class InstantDebugEffect : ItemEffect
{
    [TextArea]
    [SerializeField] private string message = "Item effect triggered";

    public override bool Apply(ItemEffectContext context)
    {
        Debug.Log(message, context.UserObject);
        return true;
    }
}

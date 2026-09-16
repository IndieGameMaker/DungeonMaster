using UnityEngine;

[CreateAssetMenu(menuName = "DungeonMaster/Item/ConsumableItemData", fileName = "ConsumableItemDataSO")]
public class ConsumableItemDataSO : ItemData
{
    // HP 회복량
    public float hpRecovery;
    // MP 회복량
    public float mpRecovery;
}

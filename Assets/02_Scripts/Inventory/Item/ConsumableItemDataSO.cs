using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "DungeonMaster/Item/ConsumableItemData", fileName = "ConsumableItemDataSO")]
public class ConsumableItemDataSO : ItemData
{
    // HP 회복량
    public float hpRecovery;
    // MP 회복량
    public float mpRecovery;
    
    public override string GetItemInfo()
    {
        // 문자열 + 문자열 => Garbage Collection 대상
        // StringBuilder C# .NET 기능

        StringBuilder sb = new StringBuilder();
        sb.Append($"HP: {hpRecovery}\n");
        sb.Append($"MP: {mpRecovery}\n");
        
        return sb.ToString();
    }
}

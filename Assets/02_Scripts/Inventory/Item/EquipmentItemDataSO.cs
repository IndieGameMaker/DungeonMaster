using System.Text;
using UnityEngine;

public enum EquipType
{
    Weapon,
    Armor,
}

[CreateAssetMenu(menuName = "DungeonMaster/Item/EquipmentItemData", fileName = "EquipmentItemDataSO")]
public class EquipmentItemDataSO : ItemData
{
    // 장비 타입
    public EquipType equipType;
    // 공격력
    public float attackDamage;
    // 방어력
    public float defense;
    // 공격 속도
    public float attackSpeed;
    
    public override string GetItemInfo()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"ATK: {attackDamage}\n");
        sb.Append($"DEF: {defense}\n");
        sb.Append($"SPD: {attackSpeed}\n");
        
        return sb.ToString();        
    }
}

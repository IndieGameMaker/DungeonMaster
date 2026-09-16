using UnityEngine;

public enum ItemType
{
    Equipment,
    Consumable,
}

public abstract class ItemData : ScriptableObject
{
    public int itemID;
    public string itemName;
    public ItemType ItemType;
    public Sprite itemIcon;
    public bool isEquip;
    [TextArea(3,10)]
    public string description;
}

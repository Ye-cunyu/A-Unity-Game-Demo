using UnityEngine;

public enum ItemType
{
    Equipment,
    Consumable,
    Other
}



[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    [Header("Identification")]
    public string itemId;  
    [Header("Basic Info")]
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public int maxStackSize = 1;
}

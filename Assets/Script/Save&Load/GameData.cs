using UnityEngine;
[System.Serializable]
public class GameData
{
    public SerializableDictionary<string, int> inventoryItems;

    public SerializableDictionary<EquipmentType, string> equippedItems;

    public int flaskStackSize;
    public Vector3 savedCheckpoint;

    public GameData()
    {
        inventoryItems = new SerializableDictionary<string, int>();
        equippedItems = new SerializableDictionary<EquipmentType, string>();
        flaskStackSize = 3; 
    }
}

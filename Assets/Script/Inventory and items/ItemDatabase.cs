// 创建一个新的 ScriptableObject 来管理所有物品
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public ItemData[] allItems;
    
    public ItemData GetItemById(string itemId)
    {
        foreach (ItemData item in allItems)
        {
            if (item.itemId == itemId)
                return item;
        }
        return null;
    }
    
    #if UNITY_EDITOR
    [ContextMenu("Collect All Items")]
    public void CollectAllItems()
    {

        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:ItemData");
        allItems = new ItemData[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
            allItems[i] = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemData>(path);
        }
    }
    #endif
}
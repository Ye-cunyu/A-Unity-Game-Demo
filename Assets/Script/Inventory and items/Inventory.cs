using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Inventory : MonoBehaviour,ISaveable
{
    public static Inventory instance;
    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    public List<InventoryItem> equipmentItems;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform consumableSlotParent;

    private UI_ItemSlot[] itemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_ItemSlot[] consumableSlot;

    [Header("Flask Settings")]
    public InventoryItem currentFlask;
    [SerializeField] private UI_ItemSlot flaskSlot;

    [Header("Flask Data")]
    [SerializeField] private ItemData_Consumable normalFlaskData;
    [SerializeField] private ItemData_Consumable emptyFlaskData;

    [SerializeField] private Player player;
    private EntityStats playerStats;
    public event Action onEquipmentChanged;

    [Header("Item Database")]
    [SerializeField] private ItemDatabase itemDatabase;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        equipmentItems = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();
        itemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        consumableSlot = consumableSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        playerStats = player.stats;

        InitializeFlask();
    }

    private void InitializeFlask()
    {
        if (normalFlaskData != null)
        {
            currentFlask = new InventoryItem(normalFlaskData);
            currentFlask.stackSize = 3;
            UpdateSlotUI();
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = item.Key;
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipmentItems.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers(playerStats);
        RemoveItem(_item);
        UpdateSlotUI();
        onEquipmentChanged?.Invoke();
    }

    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipmentItems.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers(playerStats);
        }
        UpdateSlotUI();
        onEquipmentChanged?.Invoke();
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Consumable)
        {

            if (currentFlask == null)
            {
                currentFlask = new InventoryItem(_item);
            }
            else if (currentFlask.data == _item)
            {
                currentFlask.AddStack();
            }

            else if (currentFlask.data == emptyFlaskData && _item == normalFlaskData)
            {
                currentFlask = new InventoryItem(_item);
            }
        }
        else
        {
            if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
            {
                value.AddStack();
            }
            else
            {
                InventoryItem newItem = new InventoryItem(_item);
                inventoryItems.Add(newItem);
                inventoryDictionary.Add(_item, newItem);
            }
        }
        UpdateSlotUI();
    }

    public void RemoveItem(ItemData _item)
    {

        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }
        UpdateSlotUI();
    }

    private void UpdateSlotUI()
    {

        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            itemSlot[i].UpdateSlot(inventoryItems[i]);
        }
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].SlotType)
                    equipmentSlot[i].UpdateSlot(item.Value);
            }
        }
        for (int i = 0; i < consumableSlot.Length; i++)
        {
            consumableSlot[i].CleanUpSlot();
            if (i == 0 && currentFlask != null)
            {
                consumableSlot[i].UpdateSlot(currentFlask);
            }
        }
    }

    public void UseFlask()
    {
        if (player.flaskCooldownTimer >= 0)
        {
            return;
        }
        if (currentFlask.data == emptyFlaskData)
        {            
            return;
        }
        player.flaskCooldownTimer = player.flaskCooldown;

        if (currentFlask != null && currentFlask.stackSize > 0)
        {
            

            ItemData_Consumable flaskData = currentFlask.data as ItemData_Consumable;
            if (flaskData != null)
            {
                playerStats.currentHealth += flaskData.healAmount;
                playerStats.currentHealth = Mathf.Min(playerStats.currentHealth, playerStats.maxHealth.GetValue());
                player.GetComponent<Entity>()?.TriggerHealthChanged();
                currentFlask.RemoveStack();

                if (currentFlask.stackSize <= 0)
                {
                    if (emptyFlaskData != null)
                    {
                        currentFlask = new InventoryItem(emptyFlaskData);
                        currentFlask.stackSize = 1;
                    }
                    else
                    {
                        currentFlask = null;
                    }
                }
                UpdateSlotUI();
            }
        }
    }

    public void LoadData(GameData data)
    {
        if (data == null) return;
        inventoryItems.Clear();
        inventoryDictionary.Clear();
        equipmentItems.Clear();
        equipmentDictionary.Clear();

        foreach (var itemEntry in data.inventoryItems)
        {
            string itemId = itemEntry.Key;
            int stackSize = itemEntry.Value;

            ItemData itemData = FindItemDataById(itemId);
            if (itemData != null)
            {
                InventoryItem newItem = new InventoryItem(itemData);
                newItem.stackSize = stackSize;

                inventoryItems.Add(newItem);
                inventoryDictionary.Add(itemData, newItem);
            }
        }

        foreach (var equippedEntry in data.equippedItems)
        {
            EquipmentType slotType = equippedEntry.Key;
            string itemId = equippedEntry.Value;

            ItemData itemData = FindItemDataById(itemId);
            if (itemData is ItemData_Equipment equipmentData)
            {
                EquipItemFromLoad(equipmentData);
            }
        }

        if (data.flaskStackSize > 0 && normalFlaskData != null)
        {
            currentFlask = new InventoryItem(normalFlaskData);
            currentFlask.stackSize = data.flaskStackSize;
        }
        else if (emptyFlaskData != null)
        {
            currentFlask = new InventoryItem(emptyFlaskData);
            currentFlask.stackSize = 1;
        }
        else
        {
            currentFlask = null;
        }

        UpdateSlotUI();
    }

    public void SaveData(ref GameData data)
    {
        if (data == null) return;

        data.inventoryItems.Clear();
        data.equippedItems.Clear();


        foreach (var itemEntry in inventoryDictionary)
        {
            ItemData itemData = itemEntry.Key;
            InventoryItem item = itemEntry.Value;

            if (!string.IsNullOrEmpty(itemData.itemId))
            {
                data.inventoryItems[itemData.itemId] = item.stackSize;
            }
        }

  
        foreach (var equipmentEntry in equipmentDictionary)
        {
            ItemData_Equipment equipment = equipmentEntry.Key;
            InventoryItem item = equipmentEntry.Value;

            if (!string.IsNullOrEmpty(equipment.itemId))
            {
                data.equippedItems[equipment.equipmentType] = equipment.itemId;
            }
        }


        if (currentFlask != null && currentFlask.data == normalFlaskData)
        {
            data.flaskStackSize = currentFlask.stackSize;
        }
        else
        {
            data.flaskStackSize = 0; 
        }
    }

    private ItemData FindItemDataById(string itemId)
    {
        if (itemDatabase != null)
        {
            return itemDatabase.GetItemById(itemId);
        }

   
        ItemData[] allItems = Resources.LoadAll<ItemData>("");
        foreach (ItemData item in allItems)
        {
            if (item.itemId == itemId)
                return item;
        }

        Debug.LogWarning($"Item with ID {itemId} not found!");
        return null;
    }

    
    private void EquipItemFromLoad(ItemData_Equipment equipment)
    {
        InventoryItem newItem = new InventoryItem(equipment);

       
        ItemData_Equipment oldEquipment = null;
        foreach (var item in equipmentDictionary)
        {
            if (item.Key.equipmentType == equipment.equipmentType)
                oldEquipment = item.Key;
        }

        if (oldEquipment != null)
        {
            equipmentItems.Remove(equipmentDictionary[oldEquipment]);
            equipmentDictionary.Remove(oldEquipment);
            oldEquipment.RemoveModifiers(playerStats);
        }

        equipmentItems.Add(newItem);
        equipmentDictionary.Add(equipment, newItem);
        equipment.AddModifiers(playerStats);
    }
}
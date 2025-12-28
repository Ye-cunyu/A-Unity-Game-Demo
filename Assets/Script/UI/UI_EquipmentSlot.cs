using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType SlotType;
    private void OnValidate()
    {
        gameObject.name = "Equipment Slot-" + SlotType.ToString();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;
        Inventory.instance.UnequipItem(item.data as ItemData_Equipment);
        Inventory.instance.AddItem(item.data as ItemData_Equipment);
        CleanUpSlot();
    }
}

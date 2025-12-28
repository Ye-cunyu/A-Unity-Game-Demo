using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class NewMonoBehaviourScript : UI_ItemSlot
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        Inventory.instance.UseFlask();
    }

}

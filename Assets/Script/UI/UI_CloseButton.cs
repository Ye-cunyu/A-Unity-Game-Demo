using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CloseButton : MonoBehaviour, IPointerDownHandler
{
    public event Action CloseMenu;
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        CloseMenu?.Invoke();
    }
}

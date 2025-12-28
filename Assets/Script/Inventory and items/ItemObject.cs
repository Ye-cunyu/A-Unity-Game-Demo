using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private ItemData itemData;
    private void OnValidate()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.icon;
        gameObject.name = "ItemObject_"+itemData.itemName;
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);

        }
        
    }

}

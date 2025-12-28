using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("modifier")]
    public int health;
    public int armor;
    public int damage;

    public void AddModifiers(EntityStats playerStats)
    {
        if (playerStats == null) return;


        if (health != 0)
            playerStats.maxHealth.AddModifier(health);

        if (armor != 0)
            playerStats.armor.AddModifier(armor);

        if (damage != 0)
            playerStats.damage.AddModifier(damage);
    }

    public void RemoveModifiers(EntityStats playerStats)
    {
        if (playerStats == null) return;

        if (health != 0)
            playerStats.maxHealth.modifiers.Remove(health);

        if (armor != 0)
            playerStats.armor.modifiers.Remove(armor);

        if (damage != 0)
            playerStats.damage.modifiers.Remove(damage);
    }

}
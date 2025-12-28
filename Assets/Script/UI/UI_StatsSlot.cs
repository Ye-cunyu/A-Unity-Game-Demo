using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
public enum StatType
{

    Armor,
    Damage
}

public class UI_StatsSlot : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Inventory inventory;
    private EntityStats playerStats;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    public void Start()
    {
        playerStats = player.stats;
        statNameText.text = statType.ToString();
        UpdateStatsSlot();
    }

    private void UpdateStatsSlot()
    {
        statValueText.text = GetStatValue().ToString();
    }

    private void OnValidate()
    {
        gameObject.name = statType.ToString();

    }
    private int GetStatValue()
    {
        switch (statType)
        {
            case StatType.Armor:
                return playerStats.armor.GetValue();
            case StatType.Damage:
                return playerStats.damage.GetValue();
            default:
                return 0;
        }
    }
    void OnEnable()
    {
        inventory.onEquipmentChanged += UpdateStatsSlot;
    }
    void OnDisable()
    {
        inventory.onEquipmentChanged -= UpdateStatsSlot;
    }
}

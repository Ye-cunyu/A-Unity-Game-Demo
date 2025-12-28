using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Entity entity;
    private RectTransform myTransform;
    private Slider slider;
    private EntityStats myStats;
    void Awake()
    {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<EntityStats>();
        UpdateHealth();
    }
    void OnEnable()
    {     
        
        entity.onFlipped += FlipUI;
        entity.onHealthChanged += UpdateHealth;
    }
    void Start()
    {
        
    }
    void OnDisable()
    {
        
        entity.onFlipped -= FlipUI;
        entity.onHealthChanged -= UpdateHealth;
    }
    private void FlipUI()
    {
        myTransform.Rotate(0, 180, 0);
    }
    private void UpdateHealth()
    {
        slider.maxValue = myStats.maxHealth.GetValue();
        slider.value = myStats.currentHealth;
    }
}

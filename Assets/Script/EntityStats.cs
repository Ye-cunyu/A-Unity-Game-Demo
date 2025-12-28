using UnityEngine;
using System.Collections.Generic;

public class EntityStats : MonoBehaviour
{
    public Stats maxHealth;
    public Stats armor;
    public Stats damage;
    [SerializeField] public int currentHealth;

    public virtual void Awake()
    {

        InitializeStats();

        InitializedHP();
    }

    private void InitializeStats()
    {

        maxHealth.GetValue();
        armor.GetValue();
        damage.GetValue();
    }

    public virtual void Update()
    {

    }

    public virtual void DoDamage(EntityStats _targetStats)
    {
        int totalDamage = damage.GetValue() - _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;

        GetComponent<Entity>()?.TriggerHealthChanged();
    }
    public void InitializedHP()
    {
        currentHealth = maxHealth.GetValue();
        
    }

}
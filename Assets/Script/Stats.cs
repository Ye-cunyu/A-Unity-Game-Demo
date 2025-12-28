using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Stats
{
    [SerializeField] private int baseValue;
    public List<int> modifiers;

    public int GetValue()
    {
        int finalValue = baseValue;
        if (modifiers != null)
        {
            foreach (var modifier in modifiers)
            {
                finalValue += modifier;
            }
        }
        return finalValue;
    }

    public void AddModifier(int _modifier)
    {
        if (modifiers == null)
            modifiers = new List<int>();

        modifiers.Add(_modifier);
    }

    public void RemoveModifier(int _modifier)
    {
        if (modifiers != null)
            modifiers.Remove(_modifier);
    }

    public void ClearModifiers()
    {
        if (modifiers != null)
            modifiers.Clear();
    }

    public int GetBaseValue()
    {
        return baseValue;
    }
}
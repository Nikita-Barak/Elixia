using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private int baseValue;

    public List<int> modifiers;

    public int GetValue()
    {
        var finalValue = baseValue;
        foreach (var modifier in modifiers) finalValue += modifier;
        return finalValue;
    }

    public void AddModifier(int _mod)
    {
        modifiers.Add(_mod);
    }

    public void RemoveModifier(int _mod)
    {
        modifiers.Remove(_mod);
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }
}
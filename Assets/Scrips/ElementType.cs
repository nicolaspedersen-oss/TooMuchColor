using UnityEngine;

public enum ElementType { Fire, Water, Lightning, Grass, None }

[System.Serializable]
public struct AttackHit
{
    public float damage;
    public ElementType element;

    // Elemental Effects
    public float dotDps;
    public float dotDuration;

    public float slowPercent;
    public float slowDuration;

    public float rootDuration;
}

public class ElementTypes
{
    
}

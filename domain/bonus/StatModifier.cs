namespace Lab3GameInventory.domain.bonus;

public class StatModifier
{
    public int HealthBonus{get; set;}
    public int ManaBonus{get; set;}
    //stats
    public int StrengthBonus{get; set;}
    public int IntelligenceBonus{get; set;}
    public int AgilityBonus{get; set;}
    //protection
    public int PhysicalDefenseBonus{get; set;}
    public int MagicResistanceBonus{get; set;}
    //damage
    public float PhysicalDamageMultiplier{get; set;}
    public float MagicDamageMultiplier{get; set;}

    public static StatModifier Empty => new StatModifier();
    
    public void AddStatModifier(StatModifier modifier)
    {
        HealthBonus += modifier.HealthBonus;
        ManaBonus += modifier.ManaBonus;
        StrengthBonus += modifier.StrengthBonus;
        IntelligenceBonus += modifier.IntelligenceBonus;
        AgilityBonus += modifier.AgilityBonus;
        PhysicalDefenseBonus += modifier.PhysicalDefenseBonus;
        MagicResistanceBonus += modifier.MagicResistanceBonus;
        PhysicalDamageMultiplier += modifier.PhysicalDamageMultiplier;
        MagicDamageMultiplier += modifier.MagicDamageMultiplier;
    }
}
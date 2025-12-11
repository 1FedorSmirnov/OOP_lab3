using Lab3GameInventory.domain.bonus;

namespace Lab3GameInventory.domain;

public class Weapon : Item, IEquippable
{
    public int PhysicalDamage { get; }
    public List<EquipmentSlot> EquipableSlots{ get; }
    public StatModifier Modifier { get; }
    
    public Weapon(string code, string name, ItemType type, float weight, int physicalDamage, StatModifier modifier,
        List<EquipmentSlot>? equipableSlots) : base(code, name, type, weight)
    {
        PhysicalDamage = physicalDamage;
        EquipableSlots = equipableSlots;
        var weaponDamageBonus = new StatModifier {PhysicalDamageMultiplier = physicalDamage};
        modifier.AddStatModifier(weaponDamageBonus);
        Modifier = modifier;
    }
}
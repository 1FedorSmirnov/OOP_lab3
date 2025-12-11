using Lab3GameInventory.domain.bonus;

namespace Lab3GameInventory.domain;

public class Armor : Item, IEquippable
{
    public int Defense { get; }
    public List<EquipmentSlot> EquipableSlots { get; } 
    public StatModifier Modifier { get; } 

    public Armor(string code, string name, ItemType type, float weight, int defense, StatModifier? modifier,
        List<EquipmentSlot>? equipableSlots) : base(code, name, type, weight)
    {
        Defense = defense;
        EquipableSlots = equipableSlots;
        Modifier = modifier;
    }
}
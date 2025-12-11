using Lab3GameInventory.domain.bonus;

namespace Lab3GameInventory.domain;

public interface IEquippable : IItem
{
    string Code {get;}
    ItemType Type {get;}
    float Weight {get;}
    List<EquipmentSlot> EquipableSlots {get;}
    StatModifier Modifier {get;}
    
}
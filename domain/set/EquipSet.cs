namespace Lab3GameInventory.domain.bonus;

//позволяет комбинировать
//экипированные предметы
//и получать дополнительные бонусы
//от одновременно экипированных вещей,
//входящих в сет
public class EquipSet
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public string Name {get; set;}
    public IReadOnlyCollection<string> ComponentCodes { get; set; }
    public StatModifier Bonus {get; set;}  
    
    public EquipSet(string name, StatModifier bonus, List<string> componentCodes)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ArgumentNullException.ThrowIfNull(componentCodes);
            if (componentCodes.Count < 2) throw new ArgumentException("Set definition must have at least two component");
            ComponentCodes = componentCodes.AsReadOnly();
            Bonus = bonus ?? throw new ArgumentNullException(nameof(bonus));
        }
    
}
namespace Lab3GameInventory.domain.bonus;

public class SetService: ISetService
{
    private readonly ISetProvider _setProvider;
    
    public SetService(ISetProvider setProvider)
    {
        ArgumentNullException.ThrowIfNull(setProvider);
        _setProvider = setProvider;
    }
    
    public StatModifier CalculateTotalSetBonus(HashSet<string> equipmentCodes)
    {
        ArgumentNullException.ThrowIfNull(equipmentCodes);
        
        var totalBonus = StatModifier.Empty;
        var _sets = _setProvider.GetAllSets();
        if (_sets.Count == 0) return totalBonus;
        foreach (var set in _sets)
        {
            if (IsSetEquipped(equipmentCodes, set))
            {
                totalBonus.AddStatModifier(set.Bonus);
            }
        }
        return totalBonus;
    }

    private bool IsSetEquipped(HashSet<string> equipmentCodes, EquipSet set)
    {
        return set.ComponentCodes.All(equipmentCodes.Contains);
    }
}
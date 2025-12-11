namespace Lab3GameInventory.domain.bonus;

public class InMemorySetProvider : ISetProvider
{
    private readonly List<EquipSet> _sets;
    
    public InMemorySetProvider(List<EquipSet> sets)
    {
        ArgumentNullException.ThrowIfNull(sets);
        _sets = sets;
    }
    public List<EquipSet> GetAllSets()
    {
        return _sets;
    }
}
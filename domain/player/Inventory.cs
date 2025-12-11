namespace Lab3GameInventory.domain;

public class Inventory
{
 
    private readonly List<InventoryEntry> _items = [];
    public IReadOnlyCollection<InventoryEntry> Items => _items.AsReadOnly();
    
    public void AddItem(IItem item, int amount = 1)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        var existing = _items.FirstOrDefault(s => s.Item.Code == item.Code);
        if (existing != null)
        {
            existing.Add(amount);
        }
        else
        {
            _items.Add(new InventoryEntry(item, amount));
        }
    }

    public bool RemoveItem(IItem item, int amount = 1)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        
        var targetEntry = _items.FirstOrDefault(s => s.Item.Code == item.Code);
        if (targetEntry == null) return false;
        if (!targetEntry.Remove(amount)) return false;
        if (targetEntry.Count == 0) _items.Remove(targetEntry);
        return true;
    }

    //Проверить, есть ли в инвентаре нужное количество данного предмета
    public bool HasAtLeast(IItem item, int amount)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        
        return _items.Any(s => s.Item.Code == item.Code && s.Count >= amount);  
    }

    public bool ContainsItem(IItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return _items.Exists(s => s.Item.Code == item.Code);
    }
}
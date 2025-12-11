namespace Lab3GameInventory.domain;

public class InventoryEntry
{
    public IItem Item {get;}
    public int Count {get; private set;}
    
    public InventoryEntry(IItem item, int count)
    {
        if (count < 0) throw new ArgumentException("Count cannot be negative");
        Item = item ?? throw new ArgumentNullException(nameof(item));
        Count = count;
    }
    
    public void Add(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        Count += amount;
    }

    public bool Remove(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        if (amount > Count)
            return false;

        Count -= amount;
        return true;
    }
}
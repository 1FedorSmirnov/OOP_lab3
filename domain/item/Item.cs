namespace Lab3GameInventory.domain;

public abstract class Item(string code, string name, ItemType type, float weight) : IItem
{
    public string Code {get; set;} = code ?? throw new ArgumentNullException(nameof(code));
    public string Name {get; set;} = name ?? throw new ArgumentNullException(nameof(name));
    public ItemType Type {get; set;} = type;
    public float Weight {get; set;} = weight;
}
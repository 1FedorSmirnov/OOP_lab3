using Lab3GameInventory.domain.use_effect;

namespace Lab3GameInventory.domain;

public class QuestItem(string code, string name, ItemType type, float weight) : Item(code, name, type, weight), IUsable
{
    public bool IsConsumable { get; } 
    private readonly IUseEffect _effect;
    
    public void Use(Player player)
    {
        _effect.Apply(player);
    }
}
using Lab3GameInventory.domain.use_effect;

namespace Lab3GameInventory.domain;

public class Potion(string code, string name, ItemType type, float weight) : Item(code, name, type, weight), IUsable
{
    public bool IsConsumable => true;
    private readonly IUseEffect _effect;
    public void Use(Player player)
        {
            _effect.Apply(player);
        }
}
namespace Lab3GameInventory.domain;

public interface IUsable : IItem
{
    bool IsConsumable {get;}
    void Use(Player player);
}
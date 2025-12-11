namespace Lab3GameInventory.domain.use_effect;

public class ManaEffect(int amount) : IUseEffect
{
    private int _amount {get;} = amount;

    public void Apply(Player player)
    {
        player.RestoreMana(_amount);
    }
}
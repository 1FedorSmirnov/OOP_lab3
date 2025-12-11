namespace Lab3GameInventory.domain.use_effect;

public class HealEffect(int amount) : IUseEffect
{
    private readonly int _amount = amount;

    public void Apply(Player player)
    {
        player.Heal(_amount);
    }
}
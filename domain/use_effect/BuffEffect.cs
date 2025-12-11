using Lab3GameInventory.domain.bonus;

namespace Lab3GameInventory.domain.use_effect;

public class BuffEffect(StatModifier modifier, int duration) : IUseEffect
{
    private readonly StatModifier _modifier = modifier ?? throw new ArgumentNullException(nameof(modifier));
    private readonly int _duration = duration;

    public void Apply(Player player)
    {
        var buff = new ActiveBuff(_modifier, _duration);
        player.AddBuff(buff);
    }
}
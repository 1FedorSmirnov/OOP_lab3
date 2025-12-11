namespace Lab3GameInventory.domain.use_effect;

public class QuestEffect(string questCode) : IUseEffect
{
    private readonly string _questCode = questCode ?? throw new ArgumentNullException(nameof(questCode));
    public void Apply(Player player)
    {
        player.QuestLog.CompleteQuest(_questCode);
    }
}
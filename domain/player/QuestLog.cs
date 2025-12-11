namespace Lab3GameInventory.domain;

public class QuestLog
{
    private readonly HashSet<string> _completed = [];
    public void CompleteQuest(string questCode) => _completed.Add(questCode);
    public bool IsQuestCompleted(string questCode) => _completed.Contains(questCode);
}
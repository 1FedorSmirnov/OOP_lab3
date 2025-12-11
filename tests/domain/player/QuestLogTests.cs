using Lab3GameInventory.domain;
using Xunit;

namespace Lab3GameInventory.tests.player;

public class QuestLogTests
{
    [Fact]
    public void CompleteQuest_MarksQuestAsCompleted()
    {
        // Arrange
        var log = new QuestLog();

        // Act
        log.CompleteQuest("QUEST_1");

        // Assert
        Assert.True(log.IsQuestCompleted("QUEST_1"));
    }

    [Fact]
    public void IsQuestCompleted_ReturnsFalse_ForUnknownQuest()
    {
        // Arrange
        var log = new QuestLog();

        // Act
        var completed = log.IsQuestCompleted("UNKNOWN");

        // Assert
        Assert.False(completed);
    }
}
using System.Collections.Generic;
using Lab3GameInventory.domain;
using Lab3GameInventory.domain.bonus;
using Xunit;

namespace Lab3GameInventory.tests.world;

public class GameWorldTests
{
    private class TestSetProvider(List<EquipSet> sets) : ISetProvider
    {
        public List<EquipSet> GetAllSets() => sets;
    }

    private class FakeSetService : ISetService
    {
        public StatModifier CalculateTotalSetBonus(HashSet<string> equipmentCodes)
        {
            return new StatModifier { StrengthBonus = equipmentCodes.Count };
        }
    }

    [Fact]
    public void GameWorld_ExposesPlayerAndSets()
    {
        // Arrange
        var player = new Player("Hero",
            10, 10, 10,
            5, 5,
            5.0f, 5.0f, 
            null);
        var sets = GameSets.All;
        var provider = new TestSetProvider(sets);
        var service = new FakeSetService();

        // Act
        var world = new GameWorld(player, provider, service);

        // Assert
        Assert.Same(player, world.Player);
        Assert.Equal(sets.Count, world.sets.Count);
    }
}
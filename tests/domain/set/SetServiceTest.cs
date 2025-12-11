using System.Collections.Generic;
using Lab3GameInventory.domain.bonus;
using Xunit;

namespace Lab3GameInventory.tests.set;

public class SetServiceTests
{
    [Fact]
    public void ReturnsNoBonus_WhenNoSetsEquipped()
    {
        // Arrange
        var provider = new InMemorySetProvider(GameSets.All);
        var service = new SetService(provider);
        var codes = new HashSet<string>(); // пустой набор

        // Act
        var result = service.CalculateTotalSetBonus(codes);

        // Assert
        Assert.Equal(0, result.HealthBonus);
        Assert.Equal(0, result.StrengthBonus);
        Assert.Equal(0, result.PhysicalDefenseBonus);
    }

    [Fact]
    public void ReturnsWolfSetBonus_WhenWolfSetEquipped()
    {
        // Arrange
        var provider = new InMemorySetProvider(GameSets.All);
        var service = new SetService(provider);
        var wolfCodes = new HashSet<string>(GameSets.WolfSet.ComponentCodes);

        // Act
        var result = service.CalculateTotalSetBonus(wolfCodes);

        // Assert
        Assert.Equal(GameSets.WolfSet.Bonus.StrengthBonus, result.StrengthBonus);
        Assert.Equal(GameSets.WolfSet.Bonus.PhysicalDefenseBonus, result.PhysicalDefenseBonus);
    }

    [Fact]
    public void CalculateTotalSetBonus_SumsBonuses_WhenMultipleSetsEquipped()
    {
        // Arrange
        var provider = new InMemorySetProvider(GameSets.All);
        var service = new SetService(provider);

        var allCodes = new HashSet<string>();
        foreach (var code in GameSets.WolfSet.ComponentCodes)
            allCodes.Add(code);
        foreach (var code in GameSets.BearSet.ComponentCodes)
            allCodes.Add(code);

        // Act
        var result = service.CalculateTotalSetBonus(allCodes);

        // Assert
        Assert.Equal(
            GameSets.WolfSet.Bonus.StrengthBonus + GameSets.BearSet.Bonus.StrengthBonus,
            result.StrengthBonus);

        Assert.Equal(
            GameSets.WolfSet.Bonus.HealthBonus + GameSets.BearSet.Bonus.HealthBonus,
            result.HealthBonus);

        Assert.Equal(
            GameSets.WolfSet.Bonus.PhysicalDefenseBonus + GameSets.BearSet.Bonus.PhysicalDefenseBonus,
            result.PhysicalDefenseBonus);
    }
}

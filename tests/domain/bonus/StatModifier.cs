using Lab3GameInventory.domain.bonus;
using Xunit;

namespace Lab3GameInventory.tests.bonus;

public class StatModifierTests
{
    [Fact]
    public void Empty_ReturnsNewInstanceEveryTime()
    {
        // Act
        var m1 = StatModifier.Empty;
        var m2 = StatModifier.Empty;

        // Assert
        Assert.NotSame(m1, m2);
    }

    [Fact]
    public void AddStatModifier_SumsAllFields()
    {
        // Arrange
        var baseMod = new StatModifier
        {
            HealthBonus = 10,
            ManaBonus = 5,
            StrengthBonus = 3,
            IntelligenceBonus = 2,
            AgilityBonus = 1,
            PhysicalDefenseBonus = 4,
            MagicResistanceBonus = 6,
            PhysicalDamageMultiplier = 1.0f,
            MagicDamageMultiplier = 1.0f
        };

        var other = new StatModifier
        {
            HealthBonus = 2,
            ManaBonus = 1,
            StrengthBonus = 1,
            IntelligenceBonus = 1,
            AgilityBonus = 2,
            PhysicalDefenseBonus = 1,
            MagicResistanceBonus = 1,
            PhysicalDamageMultiplier = 0.5f,
            MagicDamageMultiplier = 0.25f
        };

        // Act
        baseMod.AddStatModifier(other);

        // Assert
        Assert.Equal(12, baseMod.HealthBonus);
        Assert.Equal(6, baseMod.ManaBonus);
        Assert.Equal(4, baseMod.StrengthBonus);
        Assert.Equal(3, baseMod.IntelligenceBonus);
        Assert.Equal(3, baseMod.AgilityBonus);
        Assert.Equal(5, baseMod.PhysicalDefenseBonus);
        Assert.Equal(7, baseMod.MagicResistanceBonus);
        Assert.Equal(1.5f, baseMod.PhysicalDamageMultiplier);
        Assert.Equal(1.25f, baseMod.MagicDamageMultiplier);
    }
}
using Lab3GameInventory.domain.bonus;
using Xunit;

namespace Lab3GameInventory.tests.bonus;

public class ActiveBuffTests
{
    [Fact]
    public void DecrementTurns_ReducesRemainingTurns()
    {
        // Arrange
        var modifier = new StatModifier { StrengthBonus = 5 };
        var buff = new ActiveBuff(modifier, 3);

        // Act
        buff.DecrementTurns();

        // Assert
        Assert.Equal(2, buff.RemainingTurns);
        Assert.False(buff.IsExpired);
    }

    [Fact]
    public void IsExpired_True_WhenTurnsReachZero()
    {
        // Arrange
        var modifier = new StatModifier { StrengthBonus = 5 };
        var buff = new ActiveBuff(modifier, 1);

        // Act
        buff.DecrementTurns();

        // Assert
        Assert.True(buff.IsExpired);
    }
}
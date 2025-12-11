
using System.Reflection;
using Lab3GameInventory.builder;
using Lab3GameInventory.domain;
using Lab3GameInventory.domain.bonus;
using Xunit;

namespace Lab3GameInventory.tests.builder
{
    public class PlayerBuilderTests
    {
        private class MockSetService : ISetService
        {
            public StatModifier CalculateTotalSetBonus(HashSet<string> equipmentCodes)
            {
                return StatModifier.Empty;
            }
        }

        private static ISetService GetPrivateSetService(Player player)
        {
            var field = typeof(Player).GetField(
                "_setBonusService",
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (field == null)
                throw new InvalidOperationException("_setBonusService field not found on Player.");

            return (ISetService)field.GetValue(player)!;
        }

        [Fact]
        public void Build_UsesDefaultValues_WhenNoOverridesProvided()
        {
            // Arrange
            var builder = new PlayerBuilder();

            // Act
            var player = builder.Build();

            // Assert
            Assert.Equal("NoNameHero", player.Name);
            Assert.Equal(10, player.BaseStrength);
            Assert.Equal(10, player.BaseIntelligence);
            Assert.Equal(10, player.BaseAgility);
            Assert.Equal(5, player.BasePhysicalDefense);
            Assert.Equal(10, player.BaseMagicResistance);
            Assert.Equal(10f, player.BasePhysicalDamage);
            Assert.Equal(5f, player.BaseMagicDamage);
        }

        [Fact]
        public void WithName_OverridesDefaultName()
        {
            // Arrange
            var builder = new PlayerBuilder();

            // Act
            var player = builder
                .WithName("Archer")
                .Build();

            // Assert
            Assert.Equal("Archer", player.Name);
        }

        [Fact]
        public void WithBaseStats_OverridesBaseStrengthIntelligenceAgility()
        {
            // Arrange
            var builder = new PlayerBuilder();

            // Act
            var player = builder
                .WithBaseStats(strength: 15, intelligence: 7, agility: 12)
                .Build();

            // Assert
            Assert.Equal(15, player.BaseStrength);
            Assert.Equal(7, player.BaseIntelligence);
            Assert.Equal(12, player.BaseAgility);
        }

        [Fact]
        public void WithDamageAndDefense_OverridesCorrespondingBaseValues()
        {
            // Arrange
            var builder = new PlayerBuilder();

            // Act
            var player = builder
                .WithBasePhysicalDamage(20f)
                .WithBaseMagicDamage(8f)
                .WithBasePhysicalDefence(3)
                .WithBaseMagicResistance(6)
                .Build();

            // Assert
            Assert.Equal(20f, player.BasePhysicalDamage);
            Assert.Equal(8f, player.BaseMagicDamage);
            Assert.Equal(3, player.BasePhysicalDefense);
            Assert.Equal(6, player.BaseMagicResistance);
        }

        [Fact]
        public void WithSetBonusService_StoresServiceAndPassesItToPlayer()
        {
            // Arrange
            var builder = new PlayerBuilder();
            var setService = new MockSetService();

            // Act
            var player = builder
                .WithSetBonusService(setService)
                .Build();

            // Assert
            var internalService = GetPrivateSetService(player);
            Assert.Same(setService, internalService);
        }

        [Fact]
        public void WithSetBonusService_Throws_WhenNullPassed()
        {
            // Arrange
            var builder = new PlayerBuilder();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => builder.WithSetBonusService(null!));
        }
    }
}

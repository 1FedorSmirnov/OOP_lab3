using System;
using System.Collections.Generic;
using System.Reflection;
using Lab3GameInventory.domain;
using Lab3GameInventory.domain.bonus;
using Xunit;

namespace Lab3GameInventory.tests.player

{
    public class PlayerTests
    {
        // --- Вспомогательные заглушки ---

        // Простейший ISetService, всегда возвращает пустой модификатор.
        private class MockSetService : ISetService
        {
            public StatModifier CalculateTotalSetBonus(HashSet<string> equipmentCodes)
            {
                return StatModifier.Empty;
            }
        }
        
        // Простейший предмет, реализующий только IItem.
        private class TestItem : IItem
        {
            public string Code { get; }
            public string Name { get; }
            public float Weight { get; }

            public TestItem(string code, string name, float weight)
            {
                Code = code;
                Name = name;
                Weight = weight;
            }
        }

        // Тестовый предмет, который можно использовать (IUsable) и который расходуется.
        private class TestUsableItem : IItem, IUsable
        {
            public string Code { get; }
            public string Name { get; }
            public float Weight { get; }

            public bool IsConsumable => true;

            public int UseCallCount { get; private set; } = 0;

            public TestUsableItem(string code, string name, float weight)
            {
                Code = code;
                Name = name;
                Weight = weight;
            }

            public void Use(Player player)
            {
                // просто считаем количество вызовов
                UseCallCount++;
            }
        }

        // --- тесты ---

        [Fact]
        public void Constructor_InitializesBaseAndCurrentStats()
        {
            // Arrange
            var setService = new MockSetService();

            // Act
            var player = new Player(
                name: "Hero",
                baseStrength: 10,
                baseIntelligence: 5,
                baseAgility: 3,
                basePhysicalDefense: 2,
                baseMagicResistance: 1,
                basePhysicalDamage: 4.0f,
                baseMagicDamage: 2.0f,
                setBonusService: setService);

            // Assert
            // Базовые значения
            Assert.Equal(10, player.BaseStrength);
            Assert.Equal(5, player.BaseIntelligence);
            Assert.Equal(3, player.BaseAgility);
            Assert.Equal(2, player.BasePhysicalDefense);
            Assert.Equal(1, player.BaseMagicResistance);
            Assert.Equal(4.0f, player.BasePhysicalDamage);
            Assert.Equal(2.0f, player.BaseMagicDamage);

            //Текущие статы
            Assert.Equal(100, player.MaxHealth);   // BaseMaxHealth
            Assert.Equal(50, player.MaxMana);      // BaseMaxMana
            
            Assert.Equal(100, player.Health);      // Health = MaxHealth
            Assert.Equal(50, player.Mana);         // Mana = MaxMana

            Assert.Equal(10, player.Strength);
            Assert.Equal(5, player.Intelligence);
            Assert.Equal(2, player.PhysicalDefense);

            // Урон: base + характеристика (см. конструктор Player)
            Assert.Equal(4.0f + 10, player.PhysicalDamage);
            Assert.Equal(2.0f + 5, player.MagicDamage);
        }

        [Fact]
        public void Heal_DoesNotExceedMaxHealth()
        {
            // Arrange
            var player = new Player(
                "Hero",
                baseStrength: 10,
                baseIntelligence: 5,
                baseAgility: 3,
                basePhysicalDefense: 0,
                baseMagicResistance: 0,
                basePhysicalDamage: 4.0f,
                baseMagicDamage: 2.0f,
                setBonusService: new MockSetService());

            // Наносим урон, чтобы здоровье стало меньше максимума
            player.TakePhysicalDamage(30); // без защиты урон = 30

            var healthAfterDamage = player.Health;
            Assert.True(healthAfterDamage < player.MaxHealth);

            // Act
            player.Heal(1000); // лечим сильно больше, чем нужно

            // Assert
            Assert.Equal(player.MaxHealth, player.Health); // не больше MaxHealth
        }

        [Fact]
        public void TakePhysicalDamage_UsesPhysicalDefense()
        {
            // Arrange
            var player = new Player(
                "Hero",
                baseStrength: 10,
                baseIntelligence: 5,
                baseAgility: 3,
                basePhysicalDefense: 5,   // защита
                baseMagicResistance: 0,
                basePhysicalDamage: 4.0f,
                baseMagicDamage: 2.0f,
                setBonusService: new MockSetService());

            var startHealth = player.Health;

            // Act
            player.TakePhysicalDamage(10); // урон 10, защита 5 => фактический урон 5

            // Assert
            var expectedHealth = startHealth - 5;
            Assert.Equal(expectedHealth, player.Health);
        }

        [Fact]
        public void TakeMagicDamage_UsesMagicResistance()
        {
            // Arrange
            var player = new Player(
                "Mage",
                baseStrength: 5,
                baseIntelligence: 10,
                baseAgility: 4,
                basePhysicalDefense: 0,
                baseMagicResistance: 3, // маг. сопротивление
                basePhysicalDamage: 2.0f,
                baseMagicDamage: 5.0f,
                setBonusService: new MockSetService());

            var startHealth = player.Health;

            // Act
            player.TakeMagicDamage(10); // 10 - 3 = 7 фактического урона

            // Assert
            var expectedHealth = startHealth - 7;
            Assert.Equal(expectedHealth, player.Health);
        }

        [Fact]
        public void RestoreMana_And_SpendMana_RespectBounds()
        {
            // Arrange
            var player = new Player(
                "Mage",
                baseStrength: 5,
                baseIntelligence: 10,
                baseAgility: 4,
                basePhysicalDefense: 0,
                baseMagicResistance: 0,
                basePhysicalDamage: 2.0f,
                baseMagicDamage: 5.0f,
                setBonusService: new MockSetService());

            var maxMana = player.MaxMana;

            // Act 1: тратим ману
            player.SpendMana(30);
            var manaAfterSpend = player.Mana;
            Assert.Equal(maxMana - 30, manaAfterSpend);

            // Act 2: тратим ещё больше, чем осталось
            player.SpendMana(1000);
            var manaAfterOverSpend = player.Mana;

            // мана не должна стать отрицательной
            Assert.Equal(0, manaAfterOverSpend);

            // Act 3: восстанавливаем маны больше, чем максимум
            player.RestoreMana(1000);

            // Assert
            Assert.Equal(maxMana, player.Mana);
        }

        [Fact]
        public void AddBuff_ChangesStatsAccordingToStatModifier()
        {
            // Arrange
            var setService = new MockSetService();

            var player = new Player(
                "Hero",
                baseStrength: 10,
                baseIntelligence: 5,
                baseAgility: 3,
                basePhysicalDefense: 2,
                baseMagicResistance: 1,
                basePhysicalDamage: 4.0f,
                baseMagicDamage: 2.0f,
                setBonusService: setService);

            var baseStrength = player.Strength;
            var baseMaxHealth = player.MaxHealth;
            var basePhysicalDamage = player.PhysicalDamage;

            var mod = new StatModifier
            {
                HealthBonus = 20,
                StrengthBonus = 5,
                PhysicalDamageMultiplier = 2.0f,
                MagicDamageMultiplier = 1.0f
            };

            // Допустим, конструктор ActiveBuff: ActiveBuff(StatModifier modifier, int turns)
            var buff = new ActiveBuff(mod, remainingTurns: 3);

            // Act
            player.AddBuff(buff);

            // Assert
            Assert.Equal(baseStrength + 5, player.Strength);
            Assert.Equal(baseMaxHealth + 20, player.MaxHealth);

            // Урон умножился на 2 (с учётом формулы в Player: base + Strength, потом * multiplier)
            var damageAfterBuff = player.PhysicalDamage;
            Assert.True(damageAfterBuff > basePhysicalDamage);
        }

        [Fact]
        public void AdvanceOneTurn_RemovesExpiredBuffAndRecalculatesStats()
        {
            // Arrange
            var player = new Player(
                "Hero",
                baseStrength: 10,
                baseIntelligence: 5,
                baseAgility: 3,
                basePhysicalDefense: 2,
                baseMagicResistance: 1,
                basePhysicalDamage: 4.0f,
                baseMagicDamage: 2.0f,
                setBonusService: new MockSetService());

            var baseStrength = player.Strength;

            var mod = new StatModifier
            {
                StrengthBonus = 5
            };

            // Бафф на 1 ход
            var buff = new ActiveBuff(mod, remainingTurns: 1);

            player.AddBuff(buff);
            var strengthWithBuff = player.Strength;
            Assert.Equal(baseStrength + 5, strengthWithBuff);

            // Act: ход проходит, бафф должен истечь
            player.AdvanceOneTurn();

            // Assert: сила вернулась к базовой
            Assert.Equal(baseStrength, player.Strength);
        }

        [Fact]
        public void UseItem_ReturnsFalse_WhenItemIsNotUsable()
        {
            // Arrange
            var player = new Player(
                "Hero",
                baseStrength: 10,
                baseIntelligence: 5,
                baseAgility: 3,
                basePhysicalDefense: 2,
                baseMagicResistance: 1,
                basePhysicalDamage: 4.0f,
                baseMagicDamage: 2.0f,
                setBonusService: new MockSetService());

            var item = new TestItem("ITEM_1", "Just item", 1.0f);
            player.PickupItem(item);

            // Act
            var result = player.UseItem(item);

            // Assert
            Assert.False(result);
            Assert.True(player.Inventory.ContainsItem(item)); // предмет остался
        }

        [Fact]
        public void UseItem_UsableConsumable_AppliesEffectAndRemovesFromInventory()
        {
            // Arrange
            var player = new Player(
                "Hero",
                baseStrength: 10,
                baseIntelligence: 5,
                baseAgility: 3,
                basePhysicalDefense: 0,
                baseMagicResistance: 0,
                basePhysicalDamage: 4.0f,
                baseMagicDamage: 2.0f,
                setBonusService: new MockSetService());

            var potion = new TestUsableItem("HEAL_POTION", "Potion", 0.5f);

            player.PickupItem(potion);
            Assert.True(player.Inventory.ContainsItem(potion));

            // Act
            var used = player.UseItem(potion);

            // Assert
            Assert.True(used);
            Assert.Equal(1, potion.UseCallCount);             // эффект вызван
            Assert.False(player.Inventory.ContainsItem(potion)); // зелье исчезло (IsConsumable = true)
        }

        [Fact]
        public void EquipItem_ReturnsFalse_WhenItemIsNotEquippable()
        {
            // Arrange
            var player = new Player(
                "Hero",
                baseStrength: 10,
                baseIntelligence: 5,
                baseAgility: 3,
                basePhysicalDefense: 2,
                baseMagicResistance: 1,
                basePhysicalDamage: 4.0f,
                baseMagicDamage: 2.0f,
                setBonusService: new MockSetService());

            var item = new TestItem("ITEM_1", "Just item", 1.0f);

            // Act
            var result = player.EquipItem(item);

            // Assert
            Assert.False(result); // нельзя экипировать предмет, который не IEquippable
        }
    }
}

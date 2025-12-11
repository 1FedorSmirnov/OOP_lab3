using Lab3GameInventory.domain;
using Xunit;

namespace Lab3GameInventory.tests.player;

public class InventoryTests
{
    private class TestItem(string code, string name, float weight) : IItem
    {
        public string Code { get; } = code;
        public string Name { get; } = name;
        public float Weight { get; } = weight;
    }

    [Fact]
    public void AddItem_NewItem_AddsEntry()
    {
        // Arrange
        var inventory = new Inventory();
        var item = new TestItem("ITEM_1", "Item 1", 2.0f);

        // Act
        inventory.AddItem(item, 3);

        // Assert
        Assert.Single(inventory.Items);
        var entry = Assert.Single(inventory.Items);
        Assert.Equal(item, entry.Item);
        Assert.Equal(3, entry.Count);
    }

    [Fact]
    public void AddItem_ExistingItem_IncreasesCount()
    {
        // Arrange
        var inventory = new Inventory();
        var item = new TestItem("ITEM_1", "Item 1", 1.0f);
        inventory.AddItem(item, 2);

        // Act
        inventory.AddItem(item, 5);

        // Assert
        var entry = Assert.Single(inventory.Items);
        Assert.Equal(7, entry.Count);
        
    }

    [Fact]
    public void RemoveItem_RemovesOne()
    {
        // Arrange
        var inventory = new Inventory();
        var item = new TestItem("ITEM_1", "Item 1", 1.5f);
        inventory.AddItem(item, 3);

        // Act
        inventory.RemoveItem(item); // сигнатура в проекте: RemoveItem(IItem item)

        // Assert
        var entry = Assert.Single(inventory.Items);
        Assert.Equal(2, entry.Count);
    }

    [Fact]
    public void HasAtLeast_ReturnsTrue_WhenEnoughItems()
    {
        // Arrange
        var inventory = new Inventory();
        var item = new TestItem("ITEM_1", "Item 1", 1.0f);
        inventory.AddItem(item, 4);

        // Act
        var result = inventory.HasAtLeast(item, 3);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void HasAtLeast_ReturnsFalse_WhenNotEnoughItems()
    {
        // Arrange
        var inventory = new Inventory();
        var item = new TestItem("ITEM_1", "Item 1", 1.0f);
        inventory.AddItem(item, 2);

        // Act
        var result = inventory.HasAtLeast(item, 3);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ContainsItem_ReturnsTrue_WhenItemPresent()
    {
        // Arrange
        var inventory = new Inventory();
        var item = new TestItem("ITEM_1", "Item 1", 1.0f);
        inventory.AddItem(item);

        // Act
        var result = inventory.ContainsItem(item);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsItem_ReturnsFalse_WhenItemAbsent()
    {
        // Arrange
        var inventory = new Inventory();
        var item = new TestItem("ITEM_1", "Item 1", 1.0f);

        // Act
        var result = inventory.ContainsItem(item);

        // Assert
        Assert.False(result);
    }
}

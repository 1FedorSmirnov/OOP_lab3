namespace Lab3GameInventory.domain;

public class Equipment
{
    private static readonly Dictionary<EquipmentSlot, IEquippable?> Slots = new();
    private float Capacity { get; set; }
    
    public Equipment(float capacity = 250)
    {
        Capacity = capacity;

        foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
        {
            Slots[slot] = null;
        }
    }
    
    /// Экипировать предмет из инвентаря.
    /// - Проверяет, что предмет лежит в инвентаре.
    /// - Проверяет Capacity (с учётом того, что старые вещи с нужных слотов будут сняты).
    /// - Снимает конфликтующие предметы в инвентарь.
    /// - Перекладывает новый предмет из инвентаря в слоты.
    public bool Equip(IEquippable item, Inventory inventory)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(inventory);
        //1. Проверим, экипирован ли уже предмет
        if (GetAllEquippedItems().Contains(item)) return true;
        //2. Проверим, есть ли предмет в инвентаре
        if (!inventory.ContainsItem(item)) return false;
      
        var itemSlots = item.EquipableSlots;
        if (itemSlots == null || itemSlots.Count == 0) return false;
      
        //3. Находим все предметы, которые занимают нужные слоты
        var itemsToUnequip = new HashSet<IEquippable>();
        foreach (var slot in itemSlots)
        {
            if (Slots.TryGetValue(slot, out var existing) && existing != null && existing != item)
            {
                itemsToUnequip.Add(existing);
            }
        }
        //4. Проверим ограничение по весу
        var currentWeight = CalculateEquipmentWeight();
        var removedWeight = itemsToUnequip.Sum(i => i.Weight);
        if (currentWeight - removedWeight + item.Weight > Capacity) return false;
        // 4. Снимаем конфликтующие предметы в инвентарь и освобождаем их слоты
        foreach (var itemToUnequip in itemsToUnequip)
        {
            foreach (var pair in Slots.ToList().Where(pair => pair.Value == itemToUnequip))
            {
                Slots[pair.Key] = null;
            }
            inventory.AddItem(itemToUnequip);
        }
        // 5. Убираем новый предмет из инвентаря
        if (!inventory.RemoveItem(item))
        {
            // Что-то пошло не так — инвентарь изменился?
            return false;
        }
        // 6. Ставим новый предмет во все его слоты
        foreach (var slot in itemSlots)
        {
            Slots[slot] = item;
        }
        return true;
    }
    
    /// Снять предмет из любого одного слота:
    /// - находит предмет в этом слоте,
    /// - очищает все слоты, которые он занимал,
    /// - возвращает предмет в инвентарь.
    public bool Unequip(EquipmentSlot slot, Inventory inventory)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        //target - предмет, который занимает целевую ячейку
        if (!Slots.TryGetValue(slot, out var target) || target == null) return false;
        //предмет может занимать несколько слотов, нужно очистить все
        target.EquipableSlots.ForEach(s => Slots[s] = null);
        inventory.AddItem(target);
        return true;
    }

    private float CalculateEquipmentWeight()
    {
        return GetAllEquippedItems().Sum(i => i?.Weight ?? 0);
    }

    public static HashSet<IEquippable?> GetAllEquippedItems()
    {
        return Slots.Values.Where(v => v != null).ToHashSet();
    }
    
    public IEquippable? GetEquippedItem(EquipmentSlot slot)
    {
        return Slots[slot];
    }
}
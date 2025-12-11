using Lab3GameInventory.domain.bonus;

namespace Lab3GameInventory.domain;

public class Player
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }
    public int Level { get; private set; } = 1;
    public Inventory Inventory { get; } = new Inventory();
    public Equipment Equipment { get; } = new Equipment();
    public QuestLog QuestLog { get; } = new QuestLog();
    private readonly List<ActiveBuff> _activeBuffs = []; //для использования potion
    private readonly ISetService _setBonusService; 
    
   //BaseStats
   public int BaseMaxHealth { get; } = 100;
   public int BaseMaxMana { get; } = 50;
   public int BaseStrength{ get; }
   public int BaseIntelligence{ get; }
   public int BaseAgility{ get; }
   //protection
   public int BasePhysicalDefense {get; }
   public int BaseMagicResistance {get; }
   //damage
   public float BasePhysicalDamage {get; }
   public float BaseMagicDamage {get; }
    
    //TotaStats
    public int MaxHealth { get; set; } 
    public int MaxMana { get; set; } 
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Agility { get; set; }
    //protection
    public int PhysicalDefense {get; set;}
    public int MagicResistance {get; set;}
    //damage
    public float PhysicalDamage {get; set;}
    public float MagicDamage {get; set;}

    public Player(
        string name,
        int baseStrength,
        int baseIntelligence,
        int baseAgility,
        int basePhysicalDefense,
        int baseMagicResistance,
        float basePhysicalDamage,
        float baseMagicDamage,
        ISetService setBonusService = null)
    {
        ArgumentNullException.ThrowIfNull(name);
        Name = name;
        BaseStrength = baseStrength;
        BaseIntelligence = baseIntelligence;
        BaseAgility = baseAgility;
        BasePhysicalDefense = basePhysicalDefense;
        BaseMagicResistance = baseMagicResistance;
        BasePhysicalDamage = basePhysicalDamage;
        BaseMagicDamage = baseMagicDamage;
        if (setBonusService is not null)
        {
            _setBonusService = setBonusService;
        }
        else
        {
            var provider = new InMemorySetProvider(GameSets.All);
            _setBonusService = new SetService(provider);
        }
        
        //Инициализируем текущие статы из базовых
        MaxHealth = BaseMaxHealth;
        MaxMana = BaseMaxMana;
        
        Strength = BaseStrength;
        Intelligence = BaseIntelligence;
        Agility = BaseAgility;
        
        PhysicalDefense = BasePhysicalDefense;
        MagicResistance = BaseMagicResistance;
        
        PhysicalDamage = BasePhysicalDamage + Strength;
        MagicDamage = BaseMagicDamage + Intelligence;
        
        Health = MaxHealth;
        Mana = MaxMana;
        
    }

    public void PickupItem(IItem item)
    {
        Inventory.AddItem(item);
    }
    
    public void DropItem(IItem item)
    {
        Inventory.RemoveItem(item);
    }
    
    public void Heal(int amount)
    {
        Health = Math.Min(MaxHealth, Health + amount);
    }

    public void TakePhysicalDamage(int amount)
    {
        var damageAfterDefense = Math.Max(0, amount - PhysicalDefense);
        Health = Math.Max(0, Health - damageAfterDefense);
    }
    
    public void TakeMagicDamage(int amount)
    {
        var damageAfterDefense = Math.Max(0, amount - MagicResistance);
        Health = Math.Max(0, Health - damageAfterDefense);
    }

    public void RestoreMana(int amount)
    {
        Mana = Math.Min(MaxMana, Mana + amount);
    }

    public void SpendMana(int amount)
    {
        Mana = Math.Max(0, Mana - amount);
    }
    
    public void AddBuff(ActiveBuff buff)
    {
        _activeBuffs.Add(buff);
        RecalculateStats();
    }
    
    public void AdvanceOneTurn()
    {
        foreach (var buff in _activeBuffs)
            buff.DecrementTurns();
        _activeBuffs.RemoveAll(b => b.IsExpired);
        RecalculateStats();
    }
    
    private void ApplyStatModifier(StatModifier modifier)
    {
        MaxHealth += modifier.HealthBonus;
        MaxMana += modifier.ManaBonus;
        Strength += modifier.StrengthBonus;
        Intelligence += modifier.IntelligenceBonus;
        Agility += modifier.AgilityBonus;
        PhysicalDefense += modifier.PhysicalDefenseBonus;
        MagicResistance += modifier.MagicResistanceBonus;
        PhysicalDamage *= modifier.PhysicalDamageMultiplier;
        MagicDamage *= modifier.MagicDamageMultiplier;

        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        if (Mana > MaxMana)
        {
            Mana = MaxMana;
        }
    }
    
    private void RecalculateStats()
    {
        // Сбрасываем к базовым
        MaxHealth = BaseMaxHealth;
        MaxMana = BaseMaxMana;
        Strength = BaseStrength;
        Intelligence = BaseIntelligence;
        Agility = BaseAgility;
        PhysicalDefense = BasePhysicalDefense;
        MagicResistance = BaseMagicResistance;
        PhysicalDamage = BasePhysicalDamage + Strength;
        MagicDamage = BaseMagicDamage + Intelligence;
        //считаем статы и бонусы от экипировки
        var equipped = Equipment.GetAllEquippedItems();
        var totalEquipmentBonus = StatModifier.Empty;
        foreach (var item in equipped)
        {
            if (item is Armor armor)
            {
                PhysicalDefense += armor.Defense;
            }
            else if (item is Weapon weapon)
            {
                PhysicalDamage += weapon.PhysicalDamage;
            }
            totalEquipmentBonus.AddStatModifier(item.Modifier);
        }
        //считаем бонусы от сетов
        var equippedCodes = equipped.Select(e => e.Code).ToHashSet();
        var totalSetBonus = _setBonusService.CalculateTotalSetBonus(equippedCodes);
        
        //считаем бонусы от баффов
        var totalBuffBonus = StatModifier.Empty;
        foreach (var buff in _activeBuffs)
        {
            totalBuffBonus.AddStatModifier(buff.BuffModifier);
        }
        //суммируем все накопленные бонусы
        totalEquipmentBonus.AddStatModifier(totalSetBonus);
        totalEquipmentBonus.AddStatModifier(totalBuffBonus);
        //применяем все бонусы
        ApplyStatModifier(totalEquipmentBonus);
    }
    
     public bool EquipItem(IItem item)
     {
        ArgumentNullException.ThrowIfNull(item);
        //нельзя экипировать не экипируемый предмет
        if (item is not IEquippable equippable) return false;
        var success = Equipment.Equip(equippable, Inventory);
        if (success) RecalculateStats();
        return success;
    }

     public bool UnequipSlot(EquipmentSlot slot)
     {
         var success = Equipment.Unequip(slot, Inventory);
         if (success)
         {
             RecalculateStats();
         }
         return success;
     }

     //Попытаться использовать предмет из инвентаря
     public bool UseItem(IItem item)
     {
         ArgumentNullException.ThrowIfNull(item);
         if (item is not IUsable usable) return false;
         if (!Inventory.ContainsItem(item)) return false;
         //Применяем эффект
         usable.Use(this);
         
         //если предмет тратится,
         //то при использовании его нужно удалить из инвентаря
         if (usable.IsConsumable) Inventory.RemoveItem(item);
         return true;
     }
}
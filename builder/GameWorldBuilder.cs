using Lab3GameInventory.domain;
using Lab3GameInventory.domain.bonus;

namespace Lab3GameInventory.builder;

public class GameWorldBuilder
{
    private readonly PlayerBuilder _playerBuilder = new PlayerBuilder();
    
    private ISetProvider? _setProvider;
    private ISetService? _setService;
    
    private readonly List<IItem> _items = [];
    private readonly List<IEquippable> _equipment = [];

    public GameWorldBuilder WithPlayerName(string name)
    {
        _playerBuilder.WithName(name);
        return this;
    }

    public GameWorldBuilder WithPlayerBaseStats(
        int strength,
        int intelligence,
        int agility,
        int physicalDefense,
        int magicResistance,
        float physicalDamage,
        float magicDamage)
    {
        _playerBuilder
            .WithBaseStats(strength, intelligence, agility)
            .WithBasePhysicalDefence(physicalDefense)
            .WithBaseMagicResistance(magicResistance)
            .WithBasePhysicalDamage(physicalDamage)
            .WithBaseMagicDamage(magicDamage);

        return this;
    }
    
    public GameWorldBuilder WithSetProvider(ISetProvider setProvider)
    {
        _setProvider = setProvider ?? throw new ArgumentNullException(nameof(setProvider));
        return this;
    }

    public GameWorldBuilder WithSetService(ISetService setService)
    {
        _setService = setService ?? throw new ArgumentNullException(nameof(setService));
        return this;
    }
    
    // Добавить стартовый предмет в инвентарь игрока.
    public GameWorldBuilder AddStartingItem(IItem item)
    {
        _items.Add(item ?? throw new ArgumentNullException(nameof(item)));
        return this;
    }

  
  // Добавить экипируемый предмет в инвентарь.
  // Он будет надет на игрока.
  public GameWorldBuilder AddStartingEquippedItem(IEquippable equippable)
    {
        ArgumentNullException.ThrowIfNull(equippable);

        _equipment.Add(equippable);
        _items.Add(equippable);

        return this;
    }

    public GameWorld Build()
    {
        // 1. Создаём поставщика сетов, если не задан явно
        _setProvider ??= new InMemorySetProvider(GameSets.All);

        // 2. Создаём сервис сетов, если не задан явно
        _setService ??= new SetService(_setProvider);

        // 3. Передаём сервис сетов в PlayerBuilder
        _playerBuilder.WithSetBonusService(_setService);

        // 4. Строим игрока
        var player = _playerBuilder.Build();

        // 5. Наполняем стартовый инвентарь
        foreach (var item in _items)
        {
            player.PickupItem(item);
        }

        // 6. Экипируем стартовые предметы (если возможно)
        foreach (var equippable in _equipment)
        {
            player.EquipItem(equippable);
        }

        // 7. Собираем GameWorld
        return new GameWorld(player, _setProvider, _setService);
    }

    
}
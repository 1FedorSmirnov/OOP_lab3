using Lab3GameInventory.domain.bonus;

namespace Lab3GameInventory.domain;

public class GameWorld(Player player, ISetProvider setProvider, ISetService setService)
{
    public Player Player { get; } = player ?? throw new ArgumentNullException(nameof(player));
    private ISetProvider SetProvider { get; } = setProvider ?? throw new ArgumentNullException(nameof(setProvider));
    public ISetService SetService { get; } = setService ?? throw new ArgumentNullException(nameof(setService));

    public IReadOnlyCollection<EquipSet> sets => SetProvider.GetAllSets();
}
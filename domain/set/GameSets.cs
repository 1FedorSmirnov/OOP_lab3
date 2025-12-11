namespace Lab3GameInventory.domain.bonus;

public class GameSets
{
    public static EquipSet WolfSet { get; } =
        new EquipSet(
            name: "Wolf Set",
            componentCodes: ["WOLF_HELMET", "WOLF_ARMOR", "WOLF_BOOTS"],
            bonus: new StatModifier
            {
                StrengthBonus = 5,
                PhysicalDefenseBonus = 3
            });

    public static EquipSet BearSet { get; } =
        new EquipSet(
            name: "Bear Set",
            componentCodes: ["BEAR_HELMET", "BEAR_ARMOR", "BEAR_GLOVES"],
            bonus: new StatModifier
            {
                HealthBonus = 20,
                PhysicalDefenseBonus = 2
            });

    public static List<EquipSet> All { get; } = [WolfSet, BearSet];
}
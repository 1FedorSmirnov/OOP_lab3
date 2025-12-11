namespace Lab3GameInventory.domain.bonus;

public interface ISetService
{
    StatModifier CalculateTotalSetBonus(HashSet<string> equipmentCodes);
}
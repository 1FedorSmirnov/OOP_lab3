namespace Lab3GameInventory.domain.bonus;

//баффы - один из эффектов зелья -
//временное изменение статов игрока.
//Эффект баффа длится заданное количество ходов
public class ActiveBuff(StatModifier buffModifier, int remainingTurns)
{
    public StatModifier BuffModifier {get; set;} = buffModifier;
    public int RemainingTurns {get; private set;} = remainingTurns;

    public void DecrementTurns()
    {
        RemainingTurns--;
    }
    
    public bool IsExpired => RemainingTurns <= 0;
}
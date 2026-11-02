namespace Lab2.Buildings;

public class CommandCenter
{
    public CommandCenterPower Available { get; private set; }

    public int Level { get; }

    public CommandCenter(CommandCenterPowerTable table, int level)
    {
        Level = level;
        Available = table.GetPower(level);   // в начале доступно всё
    }

    public bool TryReserve(CommandCenterPower need)
    {
        if (!Available.CanCover(need))
            return false;                      // не хватает CPU или PG, ничего не изменилось

        Available = Available.Minus(need);
        return true;
    }
}
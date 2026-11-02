namespace Lab2.Buildings;

public sealed record CommandCenterPower(int Cpu, int PowerGrid)
{
    public bool CanCover(CommandCenterPower qwary)
    {
        if (Cpu >= qwary.Cpu && PowerGrid >= qwary.PowerGrid)
        return true;
        return false;
    }

    public CommandCenterPower Minus(CommandCenterPower qwary)
    {
        return new(Cpu - qwary.Cpu, PowerGrid - qwary.PowerGrid);
    }
}
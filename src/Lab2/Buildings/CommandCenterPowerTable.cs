namespace Lab2.Buildings;

public class CommandCenterPowerTable
{
    private readonly IReadOnlyList<CommandCenterPower> _levels =
    [
        new(1675, 6000),    // уровень 0
        new(7057, 9000),    // 1
        new(12136, 12000),  // 2
        new(17215, 15000),  // 3
        new(21315, 17000),  // 4
        new(25415, 19000),  // 5
    ];

    public int MaxLevel => _levels.Count - 1;

    public CommandCenterPower GetPower(int level)
    {
        if (level < 0 || level > MaxLevel)
            throw new ArgumentOutOfRangeException(nameof(level));
        return _levels[level];
    }
}

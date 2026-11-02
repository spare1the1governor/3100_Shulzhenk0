using Lab2.Buildings;

namespace Lab2;

public class Calculator
{
    private readonly Colony _colony;

    public Calculator(Colony colony)
    {
        _colony = colony;
    }

    public ColonyReport Calculate()
        => new(_colony.Raw.CountAll(), _colony.P1.CountAll(), _colony.P2.CountAll());
}
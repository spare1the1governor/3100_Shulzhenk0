using Lab2.Buildings;

namespace Lab2;

public class WorkCycle
{
    private readonly Colony _colony;

    public WorkCycle(Colony colony)
    {
        _colony = colony;
    }

    public void Run(int cycles)
    {
        for (int i = 0; i < cycles; i++)
        {
            foreach (Extractor extractor in _colony.Buildings.OfType<Extractor>())
                extractor.Extract();

            foreach (BaseManufacture basic in _colony.Buildings.OfType<BaseManufacture>())
                basic.Produce();

            foreach (AdvancedManufacture advanced in _colony.Buildings.OfType<AdvancedManufacture>())
                advanced.Produce();
        }
    }
}
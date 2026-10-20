using Itmo.ObjectOrientedProgramming.Lab1.Models.Minerals;

namespace Itmo.ObjectOrientedProgramming.Lab1.Models.Ores;

public class Station
{
    private const decimal PortionSize = 100;

    public static decimal CounterMinedVolume { get; private set; }

    public decimal Tax { get; }

    public Ore CurrentOre { get; }

    // Единицы руды на складе, не дошедшие до полной порции
    public decimal Warehouse { get; private set; }

    public Station(decimal tax, Ore currentOre, int minedvolume = 0)
    {
        if (tax < 0 || tax > 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(tax),
                "Налог должен быть в диапазоне от 0 до 1.");
        }

        Tax = tax;
        CurrentOre = currentOre;
        CounterMinedVolume = minedvolume;
    }

    // Принимает объём, добытый за рейс, возвращает минералы за этот рейс
    public Dictionary<Mineral, decimal> Process(decimal minedVolume)
    {
        // единицы миниралов полученные из руды
        decimal newUnits = Math.Truncate(minedVolume / CurrentOre.OreValue);
        Warehouse += newUnits;
        CounterMinedVolume += minedVolume;

        decimal portions = Warehouse / PortionSize;
        Warehouse -= portions * PortionSize;

        var mined = new Dictionary<Mineral, decimal>();

        foreach ((Mineral mineral, decimal unit) in CurrentOre.ExitOfUnit)
        {
            mined[mineral] = portions * unit;
        }

        return mined;
    }

    public (decimal Revenue, decimal RevenueAfterTax) Revenue(Dictionary<Mineral, decimal> minerals, Dictionary<Mineral, int> priceList)
    {
        decimal revenue = 0;
        foreach ((Mineral mineral, decimal amount) in minerals)
        {
            // if (!priceList.TryGetValue(mineral.ToString(), out decimal price))
            // {
            //     throw new ArgumentException(
            //         $"В прайс-листе нет цены для '{mineral}'", nameof(priceList));
            // }
            int price = priceList[mineral];

            revenue += amount * price;
        }

        decimal rTax = revenue * Tax;

        return (revenue, rTax);
    }
}
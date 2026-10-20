namespace Itmo.ObjectOrientedProgramming.Lab1.Models.Ships;

// Абстрактный базовый класс корабля. Соответствие принципу Open-closed //
public abstract class Ship
{
    public string Name { get; protected set; }

    // public decimal MiningPerCycle { get; protected set; }
    public decimal CargoCapacity { get; protected set; }

    public decimal Speed { get; protected set; }

    public int RentalRatePerHour { get; protected set; }

    protected Ship(string name, decimal cargoCapacity, decimal speed, int rentalRatePerHour)
    {
        Name = name;
        CargoCapacity = cargoCapacity;
        Speed = speed;
        RentalRatePerHour = rentalRatePerHour;
    }

    // количество руды м³ добываемое кораблём в зависимости от цикла работы
    public abstract decimal GetProductionForCycle(int cycleInTrip);
}

using Itmo.ObjectOrientedProgramming.Lab1.Models.Cargo;

namespace Itmo.ObjectOrientedProgramming.Lab1.Models.Ships;

// Information Expert - так камк хранит список кораблей, является экспертом по оценке их вренды //
public class Fleet
{
    public class ShipState
    {
        public Ship Ship { get; }

        public decimal CurrentCargo { get; set; }

        public bool IsFull => CurrentCargo >= Ship.CargoCapacity;

        public decimal FreeSpace => Ship.CargoCapacity - CurrentCargo;

        public ShipState(Ship ship)
        {
            Ship = ship;
        }
    }

    private readonly List<ShipState> _shipStates;

    public IReadOnlyList<Ship> Ships { get; }

    // Номер цикла добычи в текущем рейсе, общий для всего флота
    public int CycleInCurrentTrip { get; private set; } = 1;

    public int Length => _shipStates.Count;

    // Скорость флота равна скорости самого медленного корабля (по ТЗ)
    public decimal Speed => _shipStates.Min(s => s.Ship.Speed);

    public Fleet(Ship[] ships)
    {
        ArgumentNullException.ThrowIfNull(ships);

        if (ships.Length == 0)
            throw new ArgumentException("Не переданы корабли для флота.", nameof(ships));

        Ships = ships.ToArray();
        _shipStates = new List<ShipState>(ships.Length);

        foreach (Ship ship in ships)
        {
            _shipStates.Add(new ShipState(ship));
        }
    }

    public readonly record struct TripResult(int Hours, decimal Volume);

// canStartCycle получает часы и объём, потраченные в ЭТОМ рейсе к текущему моменту
    public TripResult RunTrip(
    ICargoStrategy strategy,
    int flightTime,
    Func<int, decimal, bool> canStartCycle) // !!! НАДО ИСПРАВИТЬ НА БОЛЕЕ ЯВНЫЙ ВАРИАНТ
{
    int hours = flightTime;      // летим в пояс
    decimal volume = 0;

    while (canStartCycle(hours, volume))
    {
        decimal mined = RunSingleCycle(strategy);

        if (mined <= 0)
            break; // никто ничего не добыл, цикл не состоялся

        volume += mined;
        hours++;                 // цикл длится 1 час
        AdvanceCycle();
    }

    hours += flightTime;         // возвращаемся
    Unload();

    return new TripResult(hours, volume);
}

// Один цикл добычи. Возвращает объём, добытый всем флотом за цикл
    private decimal RunSingleCycle(ICargoStrategy strategy)
    {
        decimal total = 0;

        foreach (ShipState state in _shipStates)
        {
            decimal potentialMining = state.Ship.GetProductionForCycle(CycleInCurrentTrip);

            if (potentialMining <= 0)
                continue;

            if (!strategy.CanPlace(_shipStates, state, potentialMining))
                continue;

            strategy.Place(_shipStates, state, potentialMining);
            total += potentialMining;
        }

        return total;
    }

    private void AdvanceCycle()
    {
        CycleInCurrentTrip++;
    }

    private void Unload()
    {
        foreach (ShipState state in _shipStates)
        {
            state.CurrentCargo = 0;
        }

        CycleInCurrentTrip = 1;
    }
}
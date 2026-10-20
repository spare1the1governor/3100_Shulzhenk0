using Itmo.ObjectOrientedProgramming.Lab1.Contract;
using Itmo.ObjectOrientedProgramming.Lab1.Models.Cargo;
using Itmo.ObjectOrientedProgramming.Lab1.Models.Minerals;
using Itmo.ObjectOrientedProgramming.Lab1.Models.Ores;
using Itmo.ObjectOrientedProgramming.Lab1.Models.Ships;

namespace Itmo.ObjectOrientedProgramming.Lab1;

/// <summary>
/// Например, класс реализующий процесс расчётов.
/// </summary>
public class Calculator
{
    public readonly record struct SimulationResult(int TotalHours, int TotalFlight,
            decimal TotalVolume,
            decimal TotalRevenue,
            decimal RentalCost,
            decimal TotalTax);

    public SimulationResult Run(
        ContractBase contract,
        Fleet fleet,
        ICargoStrategy strategy,
        Station station)
    {
        // рассчет времени полетв
        int flightTime = (int)Math.Ceiling(contract.AsteroidBelt.Distance / fleet.Speed);

        int totalHours = 0;
        decimal totalVolume = 0;
        decimal totalRevenue = 0;
        decimal totalTax = 0;
        int totalFlight = 0; // количество рейсов

        while (contract.ShouldContinue(totalHours, totalVolume))
        {
            totalFlight += 1;
            Fleet.TripResult trip = fleet.RunTrip(
                strategy,
                flightTime,
                (tripHours, tripVolume) => // замыкание что бы учитывать случаи необходимости прервать контракт в середине цикла, а не по окончании рейса
                    contract.ShouldContinue(totalHours + tripHours + flightTime, totalVolume + tripVolume));

            if (trip.Volume <= 0)
                break;

            totalHours += trip.Hours;
            totalVolume += trip.Volume;

            Dictionary<Mineral, decimal> minerals = station.Process(trip.Volume);
            (decimal revenue, decimal rTax) = station.Revenue(minerals, contract.PriceList);

            totalRevenue += revenue;
            totalTax += rTax;
        }

        decimal rentalCost = fleet.Ships.Sum(s => s.RentalRatePerHour) * totalHours;
        decimal profit = totalRevenue - rentalCost - totalTax;

        return new SimulationResult(
            totalHours,
            totalFlight,
            totalVolume,
            totalRevenue,
            rentalCost,
            totalTax);
    }
}
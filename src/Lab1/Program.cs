using Itmo.ObjectOrientedProgramming.Lab1.Contract;
using Itmo.ObjectOrientedProgramming.Lab1.Models.Cargo;
using Itmo.ObjectOrientedProgramming.Lab1.Models.Ores;

namespace Itmo.ObjectOrientedProgramming.Lab1;

public static class Program
{
    public static CalculationResult RunProgram(ContractBase contract, ICargoStrategy strategy, decimal stationTax)
    {
        ValidityResponse validity = contract.Validate();
        if (!validity.IsValid)
            return CalculationResult.Rejected(validity.NotValidReason);
        var station = new Station(stationTax, contract.AsteroidBelt.Ore);
        var calculator = new Calculator();

        Calculator.SimulationResult result = calculator.Run(contract, contract.Fleet, strategy, station);

        return CalculationResult.Success(result);
    }
}
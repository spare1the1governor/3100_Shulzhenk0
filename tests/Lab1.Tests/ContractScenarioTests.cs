using Itmo.ObjectOrientedProgramming.Lab1.Contract;
using Itmo.ObjectOrientedProgramming.Lab1.Models.AsterBelts;
using Itmo.ObjectOrientedProgramming.Lab1.Models.Cargo;
using Itmo.ObjectOrientedProgramming.Lab1.Models.Minerals;
using Itmo.ObjectOrientedProgramming.Lab1.Models.Ores;
using Itmo.ObjectOrientedProgramming.Lab1.Models.Ships;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class ContractScenarioTests
{
    [Fact(DisplayName = "Сценарий 1. Контракт на срок")]
    public void TimeContract_WithSingleVenture_IsCompleted()
    {
        var belt = new AsterBelt("Пояс A", 4, new Veldspar());
        var fleet = CreateVentureFleet();
        var contract = new TermContract(belt, fleet, CreatePriceList(), maxHours: 6);
        decimal tax = 0;
        CalculationResult result = Program.RunProgram(
            contract,
            new SharedCargoStrategy(),
            tax);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Calculator.SimulationResult simulation = result.Value.Value;
        Assert.Equal(1, simulation.TotalFlight);
        Assert.Equal(6, simulation.TotalHours);
        Assert.Equal(400m, simulation.TotalVolume);
        Assert.Equal(32_000m, simulation.TotalRevenue);
        Assert.Equal(6_000m, simulation.RentalCost);
        Assert.Equal(0m, simulation.TotalTax);
    }

    [Fact(DisplayName = "Сценарий 2. Пустой флот")]
    public void Fleet_WithoutShips_CannotBeCreated()
    {
        Assert.Throws<ArgumentException>(() => new Fleet([]));
    }

    [Fact(DisplayName = "Сценарий 3. Контракт на объём")]
    public void VolumeContract_WithSingleVenture_IsCompleted()
    {
        var belt = new AsterBelt("Пояс A", 4, new Veldspar());
        var fleet = CreateVentureFleet();
        var contract = new VolumeContract(belt, fleet, CreatePriceList(), targetVolume: 1_000);
        var simulator = new Calculator();
        decimal tax = 0;

        CalculationResult result = Program.RunProgram(
            contract,
            new SharedCargoStrategy(),
            tax);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Calculator.SimulationResult simulation = result.Value.Value;
        Assert.Equal(3, simulation.TotalFlight);
        Assert.Equal(16, simulation.TotalHours);
        Assert.Equal(1_000m, simulation.TotalVolume);
        Assert.Equal(80_000m, simulation.TotalRevenue);
        Assert.Equal(16_000m, simulation.RentalCost);
        Assert.Equal(0m, simulation.TotalTax);
    }

    [Fact(DisplayName = "Сценарий 4. Отклонение по сроку")]
    public void TimeContract_TooShort_IsRejected()
    {
        var belt = new AsterBelt("Пояс A", 4, new Veldspar());
        var contract = new TermContract(belt, CreateVentureFleet(), CreatePriceList(), maxHours: 2);

        ValidityResponse validity = contract.Validate();

        Assert.False(validity.IsValid);
        Assert.Contains("Срок контракта", validity.NotValidReason, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "Сценарий 5. Отклонение по прайс-листу")]
    public void Contract_WithIncompletePriceList_IsRejected()
    {
        var belt = new AsterBelt("Пояс B", 4, new Scordite());
        Dictionary<Mineral, int> priceList = CreatePriceList();
        priceList.Remove(new Pyerite());
        var contract = new TermContract(belt, CreateVentureFleet(), priceList, maxHours: 6);

        ValidityResponse validity = contract.Validate();

        Assert.False(validity.IsValid);
        Assert.Contains(new Pyerite().ToString(), validity.NotValidReason, StringComparison.Ordinal);
    }

    [Fact(DisplayName = "Сценарий 6. Отклонение по причине отсутствия корабля-добытчика")]
    public void Contract_WithNoMiningShip_IsInvalid()
    {
        var belt = new AsterBelt("Пояс A", 4, new Veldspar());
        var fleet = new Fleet([new Epithal("Epithal")]);
        var contract = new TermContract(belt, fleet, CreatePriceList(), maxHours: 6);

        ValidityResponse validity = contract.Validate();

        Assert.False(validity.IsValid);
        Assert.Contains("Ни один корабль", validity.NotValidReason, StringComparison.Ordinal);
    }

    [Fact]
    public void TermContract_ShouldContinueOnlyBeforeHourLimit()
    {
        var belt = new AsterBelt("Пояс A", 4, new Veldspar());
        var contract = new TermContract(belt, CreateVentureFleet(), CreatePriceList(), maxHours: 6);

        Assert.True(contract.ShouldContinue(0, 0));
        Assert.True(contract.ShouldContinue(5, 0));
        Assert.False(contract.ShouldContinue(6, 0));
    }

    [Fact]
    public void VolumeContract_ShouldContinueOnlyBeforeVolumeTarget()
    {
        var belt = new AsterBelt("Пояс A", 4, new Veldspar());
        var contract = new VolumeContract(belt, CreateVentureFleet(), CreatePriceList(), targetVolume: 1_000);

        Assert.True(contract.ShouldContinue(0, 0));
        Assert.True(contract.ShouldContinue(0, 999));
        Assert.False(contract.ShouldContinue(0, 1_000));
    }

    private static Dictionary<Mineral, int> CreatePriceList()
    {
        return new Dictionary<Mineral, int>
        {
            [new Tritanium()] = 4,
            [new Pyerite()] = 10,
            [new Mexallon()] = 70,
            [new Isogen()] = 150,
        };
    }

    private static Fleet CreateVentureFleet()
    {
        return new Fleet([new Venture("Venture")]);
    }
}

using Itmo.ObjectOrientedProgramming.Lab1.Models.Minerals;

namespace Itmo.ObjectOrientedProgramming.Lab1.Models.Ores;

public record Scordite() : Ore(
    OreValue: 0.15m,
    new Dictionary<Mineral, decimal> { { new Tritanium(), 75.0m }, { new Pyerite(), 55.0m } });

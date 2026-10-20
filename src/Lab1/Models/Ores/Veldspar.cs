using Itmo.ObjectOrientedProgramming.Lab1.Models.Minerals;

namespace Itmo.ObjectOrientedProgramming.Lab1.Models.Ores;

public record Veldspar() : Ore(OreValue: 0.1m, new Dictionary<Mineral, decimal>
{ { new Tritanium(), 200.0m } });
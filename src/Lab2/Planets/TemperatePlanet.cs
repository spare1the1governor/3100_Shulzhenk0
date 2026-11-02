using Lab2.RawMaterials;

namespace Lab2.Planets;

public record TemperatePlanet() : Planet(new Dictionary<RawMaterial, int>
{
    { new BaseMetals(), 1500 },
    { new NobleMetals(), 1500 },
    { new Microorganisms(), 1000 },
});
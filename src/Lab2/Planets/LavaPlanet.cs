using Lab2.RawMaterials;

namespace Lab2.Planets;

public record LavaPlanet() : Planet(new Dictionary<RawMaterial, int>
{
    { new BaseMetals(), 1000 },
    { new HeavyMetals(), 1500 },
    { new FelsicMagma(), 1500 },
});
using Lab2.RawMaterials;

namespace Lab2.Planets;

public record BarrenPlanet() : Planet(new Dictionary<RawMaterial, int>
{
    { new Microorganisms(), 1500 },
    { new Autotrophs(), 1500 },
    { new ComplexOrganisms(), 1000 },
});
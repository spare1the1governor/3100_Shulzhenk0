using Lab2.RawMaterials;

namespace Lab2.Planets;

public abstract record Planet(IReadOnlyDictionary<RawMaterial, int> Yields)
{
    public int RateFor(RawMaterial material) => Yields.GetValueOrDefault(material); //если материала нет на планете вернёт ArgumentNullException
}
using Lab2.Planets;
using Lab2.RawMaterials;
using Lab2.Recipes;
using Lab2.Storages;

namespace Lab2.Buildings;

// Convenience builder
public class Colony
{
    private readonly List<IBuilding> _buildings = new();

    public Planet Planet { get; }

    public CommandCenter CommandCenter { get; }

    public Storage<RawMaterial> Raw { get; }

    public Storage<P1Product> P1 { get; }

    public Storage<P2Product> P2 { get; }

    public IReadOnlyList<IBuilding> Buildings => _buildings;

    public Colony(
        Planet planet,
        CommandCenter commandCenter,
        Storage<RawMaterial> raw,
        Storage<P1Product> p1,
        Storage<P2Product> p2,
        IEnumerable<IBuilding> buildings)
    {
        Planet = planet;
        CommandCenter = commandCenter;
        Raw = raw;
        P1 = p1;
        P2 = p2;

        foreach (var building in buildings)
            Add(building);
    }

    public void Add(IBuilding building)
    {
        if (!CommandCenter.TryReserve(building.Power))
            throw new InvalidOperationException("Не хватает мощности командного центра.");
        _buildings.Add(building);
    }
}
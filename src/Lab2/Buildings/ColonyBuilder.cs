using Lab2.Planets;
using Lab2.RawMaterials;
using Lab2.Recipes;
using Lab2.Storages;

namespace Lab2.Buildings;

public class ColonyBuilder
{
    private readonly List<Func<IBuilding>> _buildings;

    private P2Recipes _p2Catalog;
    private Storage<P1Product> _p1storage;
    private Storage<P2Product> _p2storage;
    private P1Recipes _p1Catalog;
    private Planet? _planet;
    private CommandCenter? _commandCenter;
    private Storage<RawMaterial> _raw;

    public ColonyBuilder()
    {
        _planet = null;
        _commandCenter = null;
        _raw = new Storage<RawMaterial>(); // внутри билдера распределяем доступ к хранилищам
        _p1storage = new Storage<P1Product>();
        _p2storage = new Storage<P2Product>();
        _p1Catalog = new P1Recipes(); // внутри билдера передаем рецепты
        _p2Catalog = new P2Recipes();
        _buildings = new List<Func<IBuilding>>();
    }

    public ColonyBuilder WithPlanet(Planet planet)
    {
        _planet = planet;
        return this; // для цепочки вызовов
    }

    public ColonyBuilder WithCommandCenter(CommandCenter commandCenter)
    {
        _commandCenter = commandCenter;
        return this;
    }

    public ColonyBuilder WithRawStorage(Storage<RawMaterial> raw)
    {
        _raw = raw;
        return this;
    }

    public ColonyBuilder WithP1Storage(Storage<P1Product> p1storage)
    {
        _p1storage = p1storage;
        return this;
    }

    public ColonyBuilder WithP2Storage(Storage<P2Product> p2storage)
    {
        _p2storage = p2storage;
        return this;
    }

    public ColonyBuilder WithP1Catalog(P1Recipes catalog)
    {
        _p1Catalog = catalog;
        return this;
    }

    public ColonyBuilder WithP2Catalog(P2Recipes catalog)
    {
        _p2Catalog = catalog;
        return this;
    }

    public ColonyBuilder AddExtractor(RawMaterial material, int workers)
    {
        _buildings.Add(() => new Extractor(_planet!, material, workers));
        return this;
    }

    public ColonyBuilder AddBaseManufacture(P2Product target)
    {
        _buildings.Add(() => new BaseManufacture(target, _p2Catalog, _p1Catalog, _raw, _p1storage));
        return this;
    }

    public ColonyBuilder AddAdvancedManufacture(P2Product target)
    {
        _buildings.Add(() => new AdvancedManufacture(target, _p2Catalog, _p1storage, _p2storage));
        return this;
    }

    public Colony Build()
    {
        return new Colony(
            _planet ?? throw new InvalidOperationException("Не задана планета."),
            _commandCenter ?? throw new InvalidOperationException("Не задан командный центр."),
            _raw,
            _p1storage,
            _p2storage,
            _buildings.Select(create => create()).ToList());
    }
}
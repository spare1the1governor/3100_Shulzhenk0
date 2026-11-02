using Lab2.RawMaterials;
using Lab2.Recipes;
using Lab2.Storages;

namespace Lab2.Buildings;

public class BaseManufacture : IBuilding
{
    private const int RawAmount = 6000; // bазовая фабрика, за 1 ч: 6000 ед. сырья → 40 ед. товара
    private const int P1Amount = 40;
    private const int Cpu = 200;
    private const int Pg = 800;
    private readonly P2Product _target;               // какой P2 в итоге нужен
    private readonly P2Recipes _p2Catalog;        // P2 -> два P1
    private readonly P1Recipes _p1Catalog;      // сырьё <-> P1
    private readonly Storage<RawMaterial> _rawStorage;
    private readonly Storage<P1Product> _p1storage;

    public CommandCenterPower Power { get; }

    public BaseManufacture(
        P2Product target,
        P2Recipes p2Catalog,
        P1Recipes p1Catalog,
        Storage<RawMaterial> raw,
        Storage<P1Product> p1Storage)
    {
        _target = target;
        _p2Catalog = p2Catalog;
        _p1Catalog = p1Catalog;
        _rawStorage = raw;
        _p1storage = p1Storage;
        Power = new CommandCenterPower(Cpu, Pg);
    }

    public bool Produce() // полагаю будем брать amount из командного центра по мощности
    {
        // 0. какие P1 нужны для целевого P2
        P2Ingredients recipe = _p2Catalog.For(_target);

        // 1. сначала тот P1, которого на складе меньше
        var (first, second) = _p1storage.CountOf(recipe.First) <= _p1storage.CountOf(recipe.Second)
            ? (recipe.First, recipe.Second)
            : (recipe.Second, recipe.First);

        // 2. если не вышло с первым (нет сырья), пробуем второй
        return TryMake(first) || TryMake(second);
    }

    private bool TryMake(P1Product product)
    {
        RawMaterial rawMaterial = _p1Catalog.RawFor(product);   // какое сырьё нужно
        if (!_rawStorage.TryTake(rawMaterial, RawAmount))
            return false;                               // сырья не хватило, ничего не списано

        _p1storage.TryPut(product, P1Amount);
        return true;
    }
}
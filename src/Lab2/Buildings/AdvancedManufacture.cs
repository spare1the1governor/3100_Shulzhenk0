using Lab2.Recipes;
using Lab2.Storages;

namespace Lab2.Buildings;

public class AdvancedManufacture : IBuilding
{
    private const int P1Amount = 40; // продвинутая фабрика, за 1 ч: по 40 ед. каждого из двух товаров → 5 ед. товара
    private const int P2Amount = 5;
    private const int Cpu = 500;
    private const int Pg = 700;
    private readonly P2Product _target;          // какой P2 производим
    private readonly P2Recipes _catalog;     // P2 -> два P1
    private readonly Storage<P1Product> _p1storage;     // откуда берём P1
    private readonly Storage<P2Product> _p2storage;     // куда кладём P2

    public CommandCenterPower Power { get; }

    public AdvancedManufacture(
        P2Product target,
        P2Recipes catalog,
        Storage<P1Product> p1storage,
        Storage<P2Product> p2storage)
    {
        _target = target;
        _catalog = catalog;
        _p1storage = p1storage;
        _p2storage = p2storage;
        Power = new CommandCenterPower(Cpu, Pg);
    }

    public bool Produce()
    {
        P2Ingredients recipe = _catalog.For(_target);

        // списываем оба ингредиента сразу, либо ни одного
        if (!_p1storage.TryTakeAll((recipe.First, P1Amount), (recipe.Second, P1Amount)))
            return false;

        _p2storage.TryPut(_target, P2Amount);
        return true;
    }
}
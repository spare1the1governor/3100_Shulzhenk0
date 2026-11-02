using Lab2.RawMaterials;
using Lab2.Recipes;
using Lab2.Storages;

namespace Lab2.Buildings;

public interface IBuildingFactory
{
    CommandCenter CreateCommandCenter(CommandCenterPowerTable table, int level);

    Extractor CreateExtractor(RawMaterial material, int workers, Storage<RawMaterial> raw);

    BaseManufacture CreateBaseManufacture(
        P2Product target, P2Recipes p2Catalog, P1Recipes p1Catalog, Storage<RawMaterial> raw, Storage<P1Product> p1);

    AdvancedManufacture CreateAdvancedManufacture(
        P2Product target, P2Recipes p2Catalog, Storage<P1Product> p1, Storage<P2Product> p2);
}
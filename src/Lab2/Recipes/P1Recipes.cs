using Lab2.RawMaterials;

namespace Lab2.Recipes;

public class P1Recipes
{
    private readonly Dictionary<RawMaterial, P1Product> _recipes;

    public P1Recipes()
    {
        _recipes = new Dictionary<RawMaterial, P1Product>
    {
        { new BaseMetals(), new P1ReactiveMetals() },
        { new NobleMetals(), new P1PreciousMetals() },
        { new Microorganisms(), new P1Bacteria() },
        { new Autotrophs(), new P1IndustrialFibers() },
        { new ComplexOrganisms(), new P1Proteins() },
        { new HeavyMetals(), new P1ToxicMetals() },
        { new FelsicMagma(), new P1Silicon() },
    };
    }

    public P1Product GetP1(RawMaterial material)
    {
        if (_recipes.TryGetValue(material, out P1Product? product) && product is not null)
        {
            return product;
        }

        throw new ArgumentException($"Нет рецепта для {material}");
    }

    public RawMaterial RawFor(P1Product product)
{
    foreach (var pair in _recipes)
    {
        if (pair.Value == product)
            return pair.Key;
    }

    throw new ArgumentException($"Нет сырья для {product}");
}
}
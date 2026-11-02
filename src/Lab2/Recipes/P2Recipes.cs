namespace Lab2.Recipes;

public class P2Recipes
{
    private readonly Dictionary<P2Product, P2Ingredients> _recipes;

    public P2Recipes()
    {
        P2Ingredients[] recipes = new[]
        {
            new P2Ingredients(new P2MechanicalParts(),     new P1PreciousMetals(), new P1ReactiveMetals()),
            new P2Ingredients(new P2Nanites(),             new P1ReactiveMetals(), new P1Bacteria()),
            new P2Ingredients(new P2Fertilizer(),          new P1Proteins(),       new P1Bacteria()),
            new P2Ingredients(new P2ConstructionBlocks(),  new P1ToxicMetals(),    new P1ReactiveMetals()),
            new P2Ingredients(new P2MicrofiberShielding(), new P1Silicon(),        new P1IndustrialFibers()),
        };

        _recipes = recipes.ToDictionary(r => r.Key);
    }

    public P2Ingredients For(P2Product product)
    {
        if (_recipes.TryGetValue(product, out P2Ingredients? recipe))
        {
            return recipe;
        }

        throw new ArgumentException($"Нет рецепта для {product}");
    }
}
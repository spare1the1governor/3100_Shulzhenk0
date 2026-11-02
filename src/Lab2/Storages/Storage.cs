namespace Lab2.Storages;

public class Storage<T>
    where T : notnull
{
    private readonly Dictionary<T, int> _count = new();

    public int CountOf(T item)
    {
        return _count.GetValueOrDefault(item);
    }

    public IReadOnlyDictionary<T, int> CountAll()
        => new Dictionary<T, int>(_count);

    public bool TryPut(T item, int amount)
    {
        _count[item] = CountOf(item) + amount;
        return true;
    }

    public bool TryTake(T item, int amount)
    {
        if (amount > CountOf(item))
        return false;
        _count[item] = CountOf(item) - amount;
        return true;
    }

    public bool TryTakeAll(params (T item, int amount)[] items) // ???
{
    foreach (var (item, amount) in items)
    {
        if (CountOf(item) < amount)
            return false;
    }

    foreach (var (item, amount) in items)
        _count[item] = CountOf(item) - amount;

    return true;
}
}

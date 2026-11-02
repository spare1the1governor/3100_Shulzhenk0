using Lab2.Planets;
using Lab2.RawMaterials;
using Lab2.Storages;

namespace Lab2.Buildings;

// Экстрактор может добывать только сырьё, которое есть на планете его типа.
public class Extractor : IBuilding
{
    private const int BaseCpu = 400;
    private const int CpuPerWorker = 110;
    private const int BasePg = 2600;
    private const int PgPerWorker = 550;
    private readonly Planet _planet;
    private readonly RawMaterial _material;

    private readonly Storage<RawMaterial> _storage;

    public CommandCenterPower Power { get; }

    public int Workers { get; }

    public Extractor(Planet planet, RawMaterial material, int workers, Storage<RawMaterial> storage)
    {
        if (workers < 0 || workers > 10)
        {
            throw new ArgumentOutOfRangeException(
                nameof(workers),
                "worker'ов должно быть в диапазоне от 0 до 10.");
        }

        if (planet.RateFor(material) == 0)
        {
            throw new ArgumentException(
                $"{material} на этой планете не добывается.", nameof(material));
        }

        _planet = planet;
        _material = material;
        Workers = workers;
        _storage = storage;
        Power = new CommandCenterPower(
            BaseCpu + (CpuPerWorker * workers),
            BasePg + (PgPerWorker * workers));
    }

    public bool Extract(int hours = 1)
        => _storage.TryPut(_material, _planet.RateFor(_material) * Workers * hours);
}
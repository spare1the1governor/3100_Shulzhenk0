namespace Itmo.ObjectOrientedProgramming.Lab1;

public class CalculationResult
{
    public bool IsSuccess { get; }

    public Calculator.SimulationResult? Value { get; }

    public string? RejectReason { get; }

    private CalculationResult(bool isSuccess, Calculator.SimulationResult? value, string? rejectReason)
    {
        IsSuccess = isSuccess;
        Value = value;
        RejectReason = rejectReason;
    }

    public static CalculationResult Success(Calculator.SimulationResult value)
        => new CalculationResult(true, value, null);

    public static CalculationResult Rejected(string? reason)
        => new CalculationResult(false, null, reason);
}
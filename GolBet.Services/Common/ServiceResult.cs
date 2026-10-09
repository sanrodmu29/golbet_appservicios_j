namespace GolBet.Services.Common;

/// <summary>Outcome of a write operation: success, or the business rule that was broken.</summary>
public class ServiceResult
{
    public bool Success { get; private init; }
    public string? Error { get; private init; }

    public static ServiceResult Ok() => new() { Success = true };
    public static ServiceResult Fail(string error) => new() { Success = false, Error = error };
}

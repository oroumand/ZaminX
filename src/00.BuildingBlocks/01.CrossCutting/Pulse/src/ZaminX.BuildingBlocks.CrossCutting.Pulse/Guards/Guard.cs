namespace ZaminX.BuildingBlocks.CrossCutting.Pulse.Guards;

/// <summary>
/// نقطه ورود سبک برای guard clauseها.
/// </summary>
public sealed class Guard
{
    private Guard()
    {
    }

    public static Guard ThrowIf { get; } = new();
}

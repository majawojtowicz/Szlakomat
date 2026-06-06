namespace Szlakomat.Parties.Domain.Common;

public record Version(long Value)
{
    public static Version Initial() => new(0L);
    public Version Next() => new(Value + 1);
}

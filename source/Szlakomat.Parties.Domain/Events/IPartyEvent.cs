namespace Szlakomat.Parties.Domain.Events;

public interface IPartyEvent
{
    DateTimeOffset OccurredAt { get; }
}

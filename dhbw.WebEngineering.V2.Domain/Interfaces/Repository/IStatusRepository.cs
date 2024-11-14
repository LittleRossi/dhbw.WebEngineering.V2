using dhbw.WebEngineering.V2.Domain.Entities.Status;

namespace dhbw.WebEngineering.V2.Domain.Interfaces.Repository;

public interface IStatusRepository
{
    public StatusInformation GetStatus();
}

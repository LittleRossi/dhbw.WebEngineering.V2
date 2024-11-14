using dhbw.WebEngineering.V2.Domain.Entities.Status;

namespace dhbw.WebEngineering.V2.Domain.Interfaces.Service;

public interface IStatusService
{
    public StatusInformation GetStatusInformation();
}

using dhbw.WebEngineering.V2.Domain.Status;

namespace dhbw.WebEngineering.V2.Application.Services;

public class StatusService : IStatusService
{
    private readonly IStatusRepository _statusRepository;

    public StatusService(IStatusRepository statusRepository)
    {
        _statusRepository = statusRepository;
    }

    public StatusInformation GetStatusInformation()
    {
        return _statusRepository.GetStatus();
    }
}

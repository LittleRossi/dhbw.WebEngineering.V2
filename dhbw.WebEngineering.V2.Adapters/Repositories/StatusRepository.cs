using dhbw.WebEngineering.V2.Domain.Entities.Status;
using dhbw.WebEngineering.V2.Domain.Interfaces.Repository;

namespace dhbw.WebEngineering.V2.Adapters.Repositories;

public class StatusRepository : IStatusRepository
{
    public StatusInformation GetStatus()
    {
        var authorsList = new List<string> { "Nick Starzmann", "Mario Grimm", "Andreas Bauer" };
        var supportedApisList = new List<string> { "jwt-v2", "assets-v3", "reservations-v2" };

        return new StatusInformation { authors = authorsList, supportedApis = supportedApisList };
    }
}

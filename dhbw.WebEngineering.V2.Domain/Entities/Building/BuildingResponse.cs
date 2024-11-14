namespace dhbw.WebEngineering.V2.Domain.Entities.Building;

public class BuildingResponse
{
    public List<ReadBuildingDto> Buildings { get; set; } = new List<ReadBuildingDto>();

    public BuildingResponse(List<ReadBuildingDto> buildings)
    {
        Buildings = buildings;
    }
}

using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace dhbw.WebEngineering.V2.Domain.Building;

public static class BuildingMapper
{
    public static Result<Building> ToEntity(CreateBuildingDto createBuildingDto)
    {
        return Building.Create(
            name: createBuildingDto.name,
            streetname: createBuildingDto.streetname,
            housenumber: createBuildingDto.housenumber,
            country_code: createBuildingDto.country_code,
            postalcode: createBuildingDto.postalcode,
            city: createBuildingDto.city
        );
    }

    public static ReadBuildingDto ToDto(Building building)
    {
        return new ReadBuildingDto
        {
            Id = building.id,
            City = building.city,
            Country_code = building.country_code,
            Housenumber = building.housenumber,
            Name = building.name,
            Postalcode = building.postalcode,
            Streetname = building.streetname,
            Deleted_at = building.deleted_at,
        };
    }

    public static List<ReadBuildingDto> ToDto(List<Building> buildings)
    {
        List<ReadBuildingDto> result = new List<ReadBuildingDto>();

        buildings.ForEach(building => result.Add(ToDto(building)));

        return result;
    }

    public static BuildingResponse ToBuildingsResponse(List<ReadBuildingDto> buildings)
    {
        return new BuildingResponse(buildings);
    }
}

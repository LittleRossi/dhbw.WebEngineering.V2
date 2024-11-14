using dhbw.WebEngineering.V2.Domain.Entities.Building;

namespace dhbw.WebEngineering.V2.Domain.Mapper;

public static class BuildingMapper
{
    public static Building ToEntity(
        string name,
        string streetname,
        string housenumber,
        string country_code,
        string postalcode,
        string city
    )
    {
        return Building.Create(
            name: name,
            streetname: streetname,
            housenumber: housenumber,
            country_code: country_code,
            postalcode: postalcode,
            city: city
        );
    }

    public static ReadBuildingDto ToDto(Building building)
    {
        return new ReadBuildingDto
        {
            Id = building.Id,
            City = building.City,
            Country_code = building.Country_code,
            Housenumber = building.Housenumber,
            Name = building.Name,
            Postalcode = building.Postalcode,
            Streetname = building.Streetname,
            Deleted_at = building.Deleted_at,
        };
    }
}

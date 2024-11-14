using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Domain.Entities.Building;

namespace dhbw.WebEngineering.V2.Domain.Mapper;

public static class BuildingMapper
{
    public static Result<Building> ToEntity(CreateBuildingDto createBuildingDto)
    {
        #region Validation
        if (string.IsNullOrWhiteSpace(createBuildingDto.name))
        {
            return Result.Failure<Building>("Name cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(createBuildingDto.streetname))
        {
            return Result.Failure<Building>("Streetname cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(createBuildingDto.housenumber))
        {
            return Result.Failure<Building>("Housenumber cannot be empty.");
        }

        if (
            string.IsNullOrWhiteSpace(createBuildingDto.country_code)
            || createBuildingDto.country_code.Length != 2
        )
        {
            return Result.Failure<Building>("Country code must be a 2-letter code.");
        }

        if (
            string.IsNullOrWhiteSpace(createBuildingDto.postalcode)
            || !Regex.IsMatch(createBuildingDto.postalcode, @"^\d{4,10}$")
        )
        {
            return Result.Failure<Building>(
                "Postal code must be a valid numeric code between 4 to 10 digits."
            );
        }

        if (string.IsNullOrWhiteSpace(createBuildingDto.city))
        {
            return Result.Failure<Building>("City cannot be empty.");
        }

        #endregion

        return Building.Create(
            name: createBuildingDto.name,
            streetname: createBuildingDto.streetname,
            housenumber: createBuildingDto.housenumber,
            country_code: createBuildingDto.country_code,
            postalcode: createBuildingDto.postalcode,
            city: createBuildingDto.city
        );
    }

    public static Result<ReadBuildingDto> ToDto(Building building)
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

    public static Result<List<ReadBuildingDto>> ToDto(List<Building> buildings)
    {
        List<ReadBuildingDto> result = new List<ReadBuildingDto>();

        buildings.ForEach(building => result.Add(ToDto(building).Value));

        return result;
    }
}

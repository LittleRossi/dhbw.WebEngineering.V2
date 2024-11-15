using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace dhbw.WebEngineering.V2.Domain.Building;

public record Building
{
    public required Guid id { get; set; }
    public required string name { get; set; }
    public required string streetname { get; set; }
    public required string housenumber { get; set; }
    public required string country_code { get; set; }
    public required string postalcode { get; set; }
    public required string city { get; set; }

    public required DateTime? deleted_at { get; set; }

    private Building() { }

    // Constructor for JSON deserialization
    [JsonConstructor]
    public Building(
        Guid id,
        string name,
        string streetname,
        string housenumber,
        string country_code,
        string postalcode,
        string city,
        DateTime deleted_at
    )
    {
        this.id = id;
        this.name = name;
        this.streetname = streetname;
        this.housenumber = housenumber;
        this.country_code = country_code;
        this.postalcode = postalcode;
        this.city = city;
        this.deleted_at = deleted_at;
    }

    public static Result<Building> Create(
        string name,
        string streetname,
        string housenumber,
        string country_code,
        string postalcode,
        string city
    )
    {
        #region Validation
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Building>("Name cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(streetname))
        {
            return Result.Failure<Building>("Streetname cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(housenumber))
        {
            return Result.Failure<Building>("Housenumber cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(country_code) || country_code.Length != 2)
        {
            return Result.Failure<Building>("Country code must be a 2-letter code.");
        }

        if (string.IsNullOrWhiteSpace(postalcode) || !Regex.IsMatch(postalcode, @"^\d{4,10}$"))
        {
            return Result.Failure<Building>(
                "Postal code must be a valid numeric code between 4 to 10 digits."
            );
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            return Result.Failure<Building>("City cannot be empty.");
        }

        #endregion

        return new Building
        {
            id = Guid.NewGuid(),
            name = name,
            streetname = streetname,
            housenumber = housenumber,
            country_code = country_code,
            postalcode = postalcode,
            city = city,
            deleted_at = null,
        };
    }
}

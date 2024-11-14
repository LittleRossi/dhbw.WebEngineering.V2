using System.Text.Json.Serialization;

namespace dhbw.WebEngineering.V2.Domain.Entities.Building;

public record Building
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Streetname { get; set; }
    public required string Housenumber { get; set; }
    public required string Country_code { get; set; }
    public required string Postalcode { get; set; }
    public required string City { get; set; }
    public required DateTime? Deleted_at { get; set; }

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
        Id = id;
        Name = name;
        Streetname = streetname;
        Housenumber = housenumber;
        Country_code = country_code;
        Postalcode = postalcode;
        City = city;
        Deleted_at = deleted_at;
    }

    public static Building Create(
        string name,
        string streetname,
        string housenumber,
        string country_code,
        string postalcode,
        string city
    )
    {
        return new Building
        {
            Id = Guid.NewGuid(),
            Name = name,
            Streetname = streetname,
            Housenumber = housenumber,
            Country_code = country_code,
            Postalcode = postalcode,
            City = city,
            Deleted_at = null,
        };
    }
}

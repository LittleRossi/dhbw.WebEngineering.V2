using System.Text.Json.Serialization;

namespace dhbw.WebEngineering.V2.Domain.Entities.Building;

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

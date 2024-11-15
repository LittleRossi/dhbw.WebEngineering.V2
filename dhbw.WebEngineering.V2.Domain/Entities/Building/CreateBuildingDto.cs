namespace dhbw.WebEngineering.V2.Domain.Entities.Building;

public record CreateBuildingDto
{
    public required string name { get; set; }
    public required string streetname { get; set; }
    public required string housenumber { get; set; }
    public required string country_code { get; set; }
    public required string postalcode { get; set; }
    public required string city { get; set; }
}

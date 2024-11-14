namespace dhbw.WebEngineering.V2.Domain.Entities.Status;

public record StatusInformation
{
    public required List<string> authors { get; set; }
    public required List<string> supportedApis { get; set; }
}

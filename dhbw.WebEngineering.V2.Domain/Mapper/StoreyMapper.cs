using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Domain.Entities.Storey;

namespace dhbw.WebEngineering.V2.Domain.Mapper;

public class StoreyMapper
{
    public static Result<Storey> ToEntity(CreateStoreyDto createStoreyDto)
    {
        #region Validation
        if (string.IsNullOrWhiteSpace(createStoreyDto.Name))
        {
            return Result.Failure<Storey>("Name cannot be empty.");
        }

        if (createStoreyDto.Building_id == Guid.Empty)
        {
            return Result.Failure<Storey>("Building_id cannot be an empty GUID.");
        }
        #endregion

        return Storey.Create(name: createStoreyDto.Name, building_id: createStoreyDto.Building_id);
    }

    public static ReadStoreyDto ToDto(Storey storey)
    {
        return new ReadStoreyDto
        {
            Id = storey.id,
            Name = storey.name,
            Building_id = storey.building_id,
            Deleted_at = storey.deleted_at,
        };
    }

    public static List<ReadStoreyDto> ToDto(List<Storey> storeys)
    {
        var result = new List<ReadStoreyDto>();
        storeys.ForEach(storey => result.Add(ToDto(storey)));

        return result;
    }

    public static StoreyResponse ToStoreyResponse(List<ReadStoreyDto> storeys)
    {
        return new StoreyResponse(storeys);
    }
}

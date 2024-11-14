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

    public static Result<ReadStoreyDto> ToDto(Storey storey)
    {
        return new ReadStoreyDto
        {
            Id = storey.Id,
            Name = storey.Name,
            Building_id = storey.Building_id,
            Deleted_at = storey.Deleted_at,
        };
    }

    public static Result<List<ReadStoreyDto>> ToDto(List<Storey> storeys)
    {
        var result = new List<ReadStoreyDto>();
        storeys.ForEach(storey => result.Add(ToDto(storey).Value));

        return Result.Success(result);
    }
}

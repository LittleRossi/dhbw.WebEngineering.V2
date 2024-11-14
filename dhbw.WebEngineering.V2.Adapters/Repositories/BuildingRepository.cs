using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Adapters.Database;
using dhbw.WebEngineering.V2.Domain.Entities.Building;
using dhbw.WebEngineering.V2.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace dhbw.WebEngineering.V2.Adapters.Repositories;

public class BuildingRepository : IBuildingRepository
{
    private readonly AppDbContext _appDbContext;

    public BuildingRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Maybe<List<Building>>> GetAllAsync(bool includeDeleted = false)
    {
        return includeDeleted
            ? await _appDbContext.buildings.IgnoreQueryFilters().ToListAsync()
            : await _appDbContext.buildings.ToListAsync();
    }

    public async Task<Maybe<Building>> GetByIdAsync(Guid id)
    {
        return await _appDbContext.buildings.FindAsync(id);
    }

    public async Task<Maybe<Building>> CreateAsync(Building entity)
    {
        var result = await _appDbContext.buildings.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();

        return Maybe<Building>.From(result.Entity);
    }

    public async Task<Maybe<Building>> UpdateAsync(Building entity, Guid id)
    {
        var existingBuilding = await _appDbContext.buildings.FindAsync(id);

        if (existingBuilding == null)
            return null;

        existingBuilding.Name = entity.Name;
        existingBuilding.Streetname = entity.Streetname;
        existingBuilding.Housenumber = entity.Housenumber;
        existingBuilding.Country_code = entity.Country_code;
        existingBuilding.Postalcode = entity.Postalcode;
        existingBuilding.City = entity.City;
        existingBuilding.Deleted_at = entity.Deleted_at;

        await _appDbContext.SaveChangesAsync();

        return existingBuilding;
    }

    public async Task<Result> DeleteAsync(Guid id, bool permanent = false)
    {
        var building = await _appDbContext.buildings.FindAsync(id);

        if (building == null)
        {
            return Result.Failure($"No existing Building with ID: {id}");
        }

        building.Deleted_at = DateTime.UtcNow;

        await _appDbContext.SaveChangesAsync();

        return Result.Success();
    }
}

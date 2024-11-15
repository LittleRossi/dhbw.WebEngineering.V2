using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Adapters.Database;
using dhbw.WebEngineering.V2.Domain.Building;
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
        return await _appDbContext
            .buildings.IgnoreQueryFilters()
            .FirstOrDefaultAsync(b => b.id == id);
    }

    public async Task<Maybe<Building>> CreateAsync(Building entity)
    {
        var result = await _appDbContext.buildings.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();

        return Maybe<Building>.From(result.Entity);
    }

    public async Task<Maybe<Building>> UpdateAsync(Building entity, Guid id)
    {
        var existingBuilding = await _appDbContext
            .buildings.IgnoreQueryFilters()
            .FirstOrDefaultAsync(b => b.id == id);

        if (existingBuilding == null)
            return null;

        existingBuilding.name = entity.name;
        existingBuilding.streetname = entity.streetname;
        existingBuilding.housenumber = entity.housenumber;
        existingBuilding.country_code = entity.country_code;
        existingBuilding.postalcode = entity.postalcode;
        existingBuilding.city = entity.city;
        existingBuilding.deleted_at = entity.deleted_at;

        await _appDbContext.SaveChangesAsync();

        return existingBuilding;
    }

    public async Task<Result> DeleteAsync(Guid id, bool permanent = false)
    {
        var building = await _appDbContext
            .buildings.IgnoreQueryFilters()
            .FirstOrDefaultAsync(b => b.id == id);

        var storeys = await _appDbContext.storeys.ToListAsync();
        var activeStoriesOfBuilding = storeys.Where(s => s.building_id == id);

        if (building == null)
        {
            return Result.Failure($"No existing Building with ID: {id}");
        }

        if (activeStoriesOfBuilding.Any())
        {
            return Result.Failure($"Cant delete Building with active Storey");
        }

        if (permanent)
        {
            _appDbContext.buildings.Remove(building);
        }
        else
        {
            building.deleted_at = DateTime.UtcNow;
        }

        await _appDbContext.SaveChangesAsync();

        return Result.Success();
    }
}

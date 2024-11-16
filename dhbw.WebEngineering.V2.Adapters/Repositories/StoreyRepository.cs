using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Adapters.Database;
using dhbw.WebEngineering.V2.Domain.Storey;
using Microsoft.EntityFrameworkCore;

namespace dhbw.WebEngineering.V2.Adapters.Repositories;

public class StoreyRepository : IStoreyRepository
{
    private readonly AppDbContext _appDbContext;

    public StoreyRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Maybe<List<Storey>>> GetAllAsync(bool includeDeleted = false)
    {
        return includeDeleted
            ? await _appDbContext.storeys.IgnoreQueryFilters().ToListAsync()
            : await _appDbContext.storeys.ToListAsync();
    }

    public async Task<Maybe<Storey>> GetByIdAsync(Guid id)
    {
        return await _appDbContext
            .storeys.IgnoreQueryFilters()
            .FirstOrDefaultAsync(b => b.id == id);
    }

    public async Task<Maybe<Storey>> CreateAsync(Storey entity)
    {
        var building = await _appDbContext.buildings.FindAsync(entity.building_id);

        if (building == null)
            return null;

        var result = await _appDbContext.storeys.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();

        return Maybe<Storey>.From(result.Entity);
    }

    public async Task<Maybe<Storey>> UpdateAsync(Storey entity, Guid id)
    {
        var existingStorey = await _appDbContext
            .storeys.IgnoreQueryFilters()
            .FirstOrDefaultAsync(b => b.id == id);

        if (existingStorey == null)
            return null;

        existingStorey.name = entity.name;
        existingStorey.building_id = entity.building_id;
        existingStorey.deleted_at = entity.deleted_at;

        await _appDbContext.SaveChangesAsync();

        return existingStorey;
    }

    public async Task<Result> DeleteAsync(Guid id, bool permanent = false)
    {
        var storey = await _appDbContext
            .storeys.IgnoreQueryFilters()
            .FirstOrDefaultAsync(b => b.id == id);

        if (storey == null)
        {
            return Result.Failure($"No existing Storey with ID: {id}");
        }

        if (permanent)
        {
            _appDbContext.storeys.Remove(storey);
        }
        else
        {
            storey.deleted_at = DateTime.UtcNow;
        }

        try
        {
            await _appDbContext.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}

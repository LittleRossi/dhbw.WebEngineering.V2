using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Adapters.Database;
using dhbw.WebEngineering.V2.Domain.Entities.Storey;
using dhbw.WebEngineering.V2.Domain.Interfaces.Repository;
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
        return await _appDbContext.storeys.FindAsync(id);
    }

    public async Task<Maybe<Storey>> CreateAsync(Storey entity)
    {
        var result = await _appDbContext.storeys.AddAsync(entity);
        await _appDbContext.SaveChangesAsync();

        return Maybe<Storey>.From(result.Entity);
    }

    public async Task<Maybe<Storey>> UpdateAsync(Storey entity, Guid id)
    {
        var existingStorey = await _appDbContext.storeys.FindAsync(id);

        if (existingStorey == null)
            return null;

        existingStorey.Name = entity.Name;
        existingStorey.Building_id = entity.Building_id;
        existingStorey.Deleted_at = entity.Deleted_at;

        await _appDbContext.SaveChangesAsync();

        return existingStorey;
    }

    public async Task<Result> DeleteAsync(Guid id, bool permanent = false)
    {
        var storey = await _appDbContext.storeys.FindAsync(id);

        if (storey == null)
        {
            return Result.Failure($"No existing Storey with ID: {id}");
        }

        storey.Deleted_at = DateTime.UtcNow;

        await _appDbContext.SaveChangesAsync();

        return Result.Success();
    }
}

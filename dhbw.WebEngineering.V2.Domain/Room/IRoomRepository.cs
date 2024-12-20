﻿using CSharpFunctionalExtensions;

namespace dhbw.WebEngineering.V2.Domain.Room;

public interface IRoomRepository
{
    public Task<Maybe<List<Room>>> GetAllAsync(bool includeDeleted = false);
    public Task<Maybe<Room>> GetByIdAsync(Guid id);
    public Task<Maybe<Room>> CreateAsync(Room entity);
    public Task<Maybe<Room>> UpdateAsync(Room entity, Guid id);
    public Task<Result> DeleteAsync(Guid id, bool permanent = false);
}

namespace Encompass.MatchSync.Concurrent
{
    /// <summary>
    /// A handler to call when there is a conflict when deleting an item.
    /// </summary>
    /// <typeparam name="TUpdated">The type of the <paramref name="updatedItem"/></typeparam>
    /// <param name="updatedItem">The object that was deleted</param>
    /// <returns>A <see cref="ConflictResolution"/></returns>
    public delegate ConflictResolution DeletedItemConflictHandler<in TUpdated>(TUpdated updatedItem);
}

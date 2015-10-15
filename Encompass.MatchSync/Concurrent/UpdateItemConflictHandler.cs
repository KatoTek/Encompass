namespace Encompass.MatchSync.Concurrent
{
    /// <summary>
    /// A handler to call when there is a conflict when updating an item
    /// </summary>
    /// <typeparam name="TSource">The type of the <paramref name="updatedSourceItem"/></typeparam>
    /// <typeparam name="TWorking">The type of the <paramref name="updatedWorkingItem"/></typeparam>
    /// <param name="updatedSourceItem">The object that was used as the source when updating</param>
    /// <param name="updatedWorkingItem">The object that was updated</param>
    /// <returns>A <see cref="ConflictResolution"/></returns>
    public delegate ConflictResolution UpdateItemConflictHandler<in TSource, in TWorking>(TSource updatedSourceItem, TWorking updatedWorkingItem);
}

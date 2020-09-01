namespace Geek.ProjectCore.Domain.Model
{
    public class  EntityBase<TKey> : Entity<TKey>
    {
        // IEntityVersion, IEntitySoftDelete, IEntityAdd<TKey>, IEntityUpdate<TKey>
    }

    public class EntityBase : EntityBase<long>
    {

    }
}

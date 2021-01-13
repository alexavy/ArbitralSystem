using ArbitralSystem.Domain.Common;

namespace ArbitralSystem.Repository.BulkRepository
{
    public interface IBulkRepository<in T>  where T : class , IArbitralObject
    {
        void Save(T[] objs);
    }
}

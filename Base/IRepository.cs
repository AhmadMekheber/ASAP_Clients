namespace ASAP_Clients.Base
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        void Add(T entity);

        int SaveChanges();

        Task<int> SaveChangesAsync();

        void BulkSaveChanges();
    }
}
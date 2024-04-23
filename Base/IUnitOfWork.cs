using ASAP_Clients.Data;
using ASAP_Clients.Repository.IRepository;

namespace ASAP_Clients.Base
{
    public interface IUnitOfWork : IDisposable
    {
        DataContext DbContext { get; }

        // Method to save changes to the database
        int SaveChanges();

        Task<int> SaveChangesAsync();

        void BulkSaveChanges();

        Task BulkSaveChangesAsync();
    }
}
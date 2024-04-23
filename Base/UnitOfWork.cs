using ASAP_Clients.Data;
using ASAP_Clients.Repository.IRepository;
using EFCore.BulkExtensions;

namespace ASAP_Clients.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dbContext;

        public UnitOfWork(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DataContext DbContext => _dbContext;

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void BulkSaveChanges()
        {
            _dbContext.BulkSaveChanges();
        }

        public async Task BulkSaveChangesAsync()
        {
            await _dbContext.BulkSaveChangesAsync();
        }
        //Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this._dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
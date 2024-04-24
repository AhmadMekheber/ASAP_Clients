using ASAP_Clients.Base;
using ASAP_Clients.Data;
using ASAP_Clients.Entities;
using ASAP_Clients.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ASAP_Clients.Repository
{
    public class PreviousCloseResponseRepository : EFRepository<PreviousCloseResponse>, IPreviousCloseResponseRepository
    {
        public PreviousCloseResponseRepository(DataContext context) : base(context)
        {
        }

        public IQueryable<PreviousCloseResponse> GetResponsesToNotify()
        {
            return GetAll()
                .Where(response => response.IsClientsNotified == false)
                .Include(response => response.Request)
                .ThenInclude(request => request.Ticker);
        }
    }
}
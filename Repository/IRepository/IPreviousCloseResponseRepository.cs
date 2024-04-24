using ASAP_Clients.Base;
using ASAP_Clients.Entities;

namespace ASAP_Clients.Repository.IRepository
{
    public interface IPreviousCloseResponseRepository : IRepository<PreviousCloseResponse>
    {
        IQueryable<PreviousCloseResponse> GetResponsesToNotify();
    }
}
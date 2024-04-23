using ASAP_Clients.Base;
using ASAP_Clients.Data;
using ASAP_Clients.Entities;
using ASAP_Clients.Repository.IRepository;

namespace ASAP_Clients.Repository
{
    public class PreviousCloseResponseRepository : EFRepository<PreviousCloseResponse>, IPreviousCloseResponseRepository
    {
        public PreviousCloseResponseRepository(DataContext context) : base(context)
        {
        }
    }
}
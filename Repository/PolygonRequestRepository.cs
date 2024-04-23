using ASAP_Clients.Base;
using ASAP_Clients.Data;
using ASAP_Clients.Entities;
using ASAP_Clients.Repository.IRepository;

namespace ASAP_Clients.Repository
{
    public class PolygonRequestRepository : EFRepository<PolygonRequest>, IPolygonRequestRepository
    {
        public PolygonRequestRepository(DataContext context) : base(context)
        {
        }
    }
}
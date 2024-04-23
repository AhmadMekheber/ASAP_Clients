using ASAP_Clients.Base;
using ASAP_Clients.Data;
using ASAP_Clients.Repository.IRepository;

namespace ASAP_Clients.Repository
{
    public class ClientsUnitOfWork : UnitOfWork, IClientsUnitOfWork
    {
        private readonly IClientRepository _clientRepository;
        private readonly IPolygonTickerRepository _polygonTickerRepository;
        private readonly IPolygonRequestRepository _polygonRequestRepository;
        private readonly IPreviousCloseResponseRepository _previousCloseResponseRepository;

        public ClientsUnitOfWork(DataContext dbContext,
            IClientRepository clientRepository,
            IPolygonTickerRepository polygonTickerRepository,
            IPolygonRequestRepository polygonRequestRepository,
            IPreviousCloseResponseRepository previousCloseResponseRepository) : base(dbContext)
        {
            _clientRepository = clientRepository;
            _polygonTickerRepository = polygonTickerRepository;
            _polygonRequestRepository = polygonRequestRepository;
            _previousCloseResponseRepository = previousCloseResponseRepository;
        }

        public IClientRepository ClientRepository => _clientRepository;

        public IPolygonTickerRepository PolygonTickerRepository => _polygonTickerRepository;

        public IPolygonRequestRepository PolygonRequestRepository => _polygonRequestRepository;

        public IPreviousCloseResponseRepository PreviousCloseResponseRepository => _previousCloseResponseRepository;
    }
}
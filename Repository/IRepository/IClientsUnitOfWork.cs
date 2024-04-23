using ASAP_Clients.Base;

namespace ASAP_Clients.Repository.IRepository
{
    public interface IClientsUnitOfWork : IUnitOfWork
    {
        IClientRepository ClientRepository { get; }

        IPolygonTickerRepository PolygonTickerRepository { get; }

        IPolygonRequestRepository PolygonRequestRepository { get; }
        
        IPreviousCloseResponseRepository PreviousCloseResponseRepository { get; }
    }
}
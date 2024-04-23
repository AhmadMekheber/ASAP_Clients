using ASAP_Clients.Base;
using ASAP_Clients.Entities;

namespace ASAP_Clients.Repository.IRepository
{
    public interface IClientRepository : IRepository<Client>
    {
        IQueryable<Client> GetNotDeleted();

        Task<Client?> GetByID(long clientID);

        Task<bool> EmailExists(string email, long? clientID);
    }
}
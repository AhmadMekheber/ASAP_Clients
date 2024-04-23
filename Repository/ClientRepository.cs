using ASAP_Clients.Base;
using ASAP_Clients.Data;
using ASAP_Clients.Entities;
using ASAP_Clients.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ASAP_Clients.Repository
{
    public class ClientRepository : EFRepository<Client>, IClientRepository
    {
        public ClientRepository(DataContext context) : base(context)
        {
        }

        public async Task<Client?> GetByID(long clientID)
        {
            return await GetNotDeleted().FirstOrDefaultAsync(client => client.ID == clientID);
        }

        public IQueryable<Client> GetNotDeleted() 
        {
            return GetAll().Where(client => client.IsDeleted == false);
        }

        public async Task<bool> EmailExists(string email, long? clientID)
        {
            string emailLowercase = email.ToLower();

            var query = GetNotDeleted()
                .Where(client => client.Email.ToLower() == emailLowercase);

            if (clientID.HasValue)
            {
                query = query.Where(client => client.ID != clientID);
            }

            return await query.AnyAsync();
        }
    }
}
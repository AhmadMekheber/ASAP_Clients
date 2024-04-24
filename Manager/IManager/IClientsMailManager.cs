namespace ASAP_Clients.Manager.IManager
{
    public interface IClientsMailManager
    {
        Task SendMailsToUnnotifiedClients();
    }
}
using ASAP_Clients.Entities;
using ASAP_Clients.PolygonEntities;

namespace ASAP_Clients.Manager.IManager
{
    public interface IPolygonManager 
    {
        Task<List<PolygonTicker>> GetPolygonTickers();

        Task SavePreviousCloses(List<(long tickerID, PreviousClose previousClose)> previousCloses);
    }
}
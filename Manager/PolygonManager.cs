using System.Collections.ObjectModel;
using ASAP_Clients.Entities;
using ASAP_Clients.Manager.IManager;
using ASAP_Clients.PolygonEntities;
using ASAP_Clients.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ASAP_Clients.Manager
{
    public class PolygonManager : IPolygonManager
    {
        private readonly IClientsUnitOfWork _unitOfWork;

        private readonly IPolygonTickerRepository _polygonTickerReopsitory;

        public PolygonManager(IClientsUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _polygonTickerReopsitory = _unitOfWork.PolygonTickerRepository;
        }

        public async Task<List<PolygonTicker>> GetPolygonTickers() 
        {
            return await _polygonTickerReopsitory.GetAll().ToListAsync();
        }

        public async Task SavePreviousCloses(List<(long tickerID, PreviousClose previousClose)> previousCloses) 
        {
            foreach((long tickerID, PreviousClose previousClose) item in previousCloses)
            {
                AddPreviousClose(item.tickerID, item.previousClose);
            }

            if (previousCloses.Any())
            {
                await _unitOfWork.BulkSaveChangesAsync();
            }
        }

        private void AddPreviousClose(long tickerID, PreviousClose previousClose) 
        {
            Guid requestID = Guid.Parse(previousClose.request_id);
            
            AddPolygonRequest(tickerID, requestID);

            foreach (var item in previousClose.results)
            {
                AddPreviousCloseResult(requestID, item);    
            }
        }

        private void AddPolygonRequest(long tickerID, Guid requestID)
        {
            PolygonRequest polygonRequest = new PolygonRequest
            {
                ID = requestID,
                RequestTypeID = (int)Enums.PolygonRequestType.PreviousClose,
                TickerID = tickerID,
                RequestedAt = DateTime.UtcNow
            };
            
            _unitOfWork.PolygonRequestRepository.Add(polygonRequest);
        }
        
        private void AddPreviousCloseResult(Guid requestID, PreviousCloseResult previousCloseResult)
        {
            DateTimeOffset offset = DateTimeOffset.FromUnixTimeMilliseconds(previousCloseResult.t);

            var previousCloseResponse = new PreviousCloseResponse
            {
                RequestID = requestID,
                ClosePrice = previousCloseResult.c,
                HighestPrice = previousCloseResult.h,
                LowestPrice = previousCloseResult.l,
                OpenPrice = previousCloseResult.o,
                VolumeWeightedAveragePrice = previousCloseResult.vw,
                TransactionsCount = previousCloseResult.n,
                TradingVolume = previousCloseResult.v,
                AggregateWindowDate = offset.UtcDateTime,
                IsClientsNotified = false
            };

            _unitOfWork.PreviousCloseResponseRepository.Add(previousCloseResponse);
        }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ASAP_Clients.Entities
{
    public class PreviousCloseResponse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        public Guid RequestID { get; set; }

        [Precision(16, 4)]
        public decimal ClosePrice { get; set; }

        [Precision(16, 4)]
        public decimal HighestPrice { get; set; }

        [Precision(16, 4)]
        public decimal LowestPrice { get; set; }

        [Precision(16, 4)]
        public decimal OpenPrice { get; set; }

        [Precision(16, 4)]
        public decimal VolumeWeightedAveragePrice { get; set; }

        public long TransactionsCount { get; set; }

        [Precision(18, 2)]
        public decimal TradingVolume { get; set; }

        public DateTime AggregateWindowDate { get; set; }
        
        public required bool IsClientsNotified { get; set; } = false;

        //
        //Navigation Properties
        //
        [ForeignKey(nameof(RequestID))]
        public virtual PolygonRequest? Request { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASAP_Clients.Entities
{
    public class PolygonRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ID { get; set; }

        public int RequestTypeID { get; set; }

        public long TickerID { get; set; }
        
        public DateTime RequestedAt { get; set; }

        //
        //Navigation Properties
        //
        [ForeignKey(nameof(RequestTypeID))]
        public virtual PolygonRequestType? RequestType { get; set; }

        [ForeignKey(nameof(TickerID))]
        public virtual PolygonTicker? Ticker { get; set; }

        public virtual ICollection<PreviousCloseResponse>? PreviousCloseResponses { get; set; }
    }
}
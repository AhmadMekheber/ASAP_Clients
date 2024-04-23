using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASAP_Clients.Entities
{
    public class PolygonTicker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [StringLength(250)]
        public required string Name { get; set; }

        [StringLength(250)]
        public required string CompanyName { get; set; }

        //
        //Navigation Properties
        //
        public virtual ICollection<PolygonRequest>? PolygonRequests { get; set; }
    }
}
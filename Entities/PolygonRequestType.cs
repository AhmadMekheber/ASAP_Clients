using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASAP_Clients.Entities
{
    public class PolygonRequestType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(250)]
        public required string Name { get; set; }

        //
        //Navigation Properties
        //
        public virtual ICollection<PolygonRequest>? PolygonRequests { get; set; }
    }
}
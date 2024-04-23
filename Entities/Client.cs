using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASAP_Clients.Entities
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [StringLength(50)]
        public required string FirstName { get; set; }

        [StringLength(50)]
        public required string LastName { get; set; }

        [StringLength(320)]
        public required string Email { get; set; }

        [StringLength(20)]
        public required string PhoneNumber { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
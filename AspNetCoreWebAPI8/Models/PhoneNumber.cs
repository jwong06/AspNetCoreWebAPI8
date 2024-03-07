using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace AspNetCoreWebAPI8.Models
{
    public class PhoneNumber
    {
        public required int Id { get; set; }
        [StringLength(11, MinimumLength = 11)]
        public required string PhoneNo { get; set; }

        [ForeignKey("AccountId")]
        public int? AccountId { get; set; }
        public Account? Account { get; private set; }
    }

}

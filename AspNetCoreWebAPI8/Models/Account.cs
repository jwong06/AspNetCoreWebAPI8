using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreWebAPI8.Models
{
    public class Account
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public bool Status { get; set; }
        //public List<PhoneNumber>? PhoneNumbers { get; set; }
    }
}

using Microblink.Library.Services.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;



namespace Microblink.Library.Services.Models.Dto
{
    [Table("Contacts")]
    public partial class Contact : IContact
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ContactTypeId { get; set; }
        public string Value { get; set; }
        public virtual ContactType ContactType { get; set; }
        public virtual User User { get; set; }

    }
}

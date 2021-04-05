

using Microblink.Library.Services.Models.Dto.Interfaces;

namespace Microblink.Library.Api.Models
{
    ///<inheritdoc/>
    public class Contact : IContact
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ContactTypeId { get; set; }
        public string Value { get; set; }
    }
}

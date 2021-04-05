using Microblink.Library.Services.Models.Dto.Interfaces;

namespace Microblink.Library.Services.Models.Dto
{
    /// <inheritdoc/>
    public class ContactDto : IContact
    {
        public int Id { get; set; }        
        public int UserId { get; set; }
        public int ContactTypeId { get; set; }
        public string Value { get; set; }
    }
}



using Microblink.Library.Services.Models.Dto.Interfaces;

namespace Microblink.Library.Api.Models
{
    /// <inheritdoc/>
    public class ContactType : IContactType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
    }
}

using Microblink.Library.Services.Models.Dto.Interfaces;

namespace Microblink.Library.Services.Models.Dto
{
    /// <inheritdoc/>
    public class ContactTypeDto : IContactType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
    }
}

using Microblink.Library.Services.Models.Dto.Interfaces;

namespace Microblink.Library.Api.Models
{
    /// <inheritdoc/>
    public class Book : IBook
    {
        public int Id { get; set; }
        public int BookTitleId { get; set; }
        public string Code { get; set; }
    }
}

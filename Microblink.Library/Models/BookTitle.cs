using Microblink.Library.Services.Models.Dto.Interfaces;

namespace Microblink.Library.Api.Models
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class BookTitle : IBookTitle
    {
        public int Id { get; set; }
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public int TotalCount { get; set; }
        public int AvailableCount => TotalCount - RentedCount;
        public int RentedCount { get; set; }
    }
}

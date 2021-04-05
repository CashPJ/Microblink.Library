using Microblink.Library.Services.Models.Dto.Interfaces;

namespace Microblink.Library.Services.Models.Dto
{
    /// <summary>
    /// Book title 
    /// </summary>
    public class BookTitleDto : IBookTitle
    {
        public int Id { get; set; }
        public string Isbn { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public int TotalCount { get; set; }        
        public int RentedCount { get; set; }
    }
}

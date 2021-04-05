using Microblink.Library.Services.Models.Dto.Interfaces;

namespace Microblink.Library.Services.Models.Dto
{
    public class BookDto : IBook
    {
        public int Id { get; set; }
        public int BookTitleId { get; set; }
        public string Code { get; set; }
    }
}

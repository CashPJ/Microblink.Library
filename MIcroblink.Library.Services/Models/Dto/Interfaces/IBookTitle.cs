namespace Microblink.Library.Services.Models.Dto.Interfaces
{
    public interface IBookTitle
    {
        int Id { get; set; }
        string Isbn { get; set; }
        string Author { get; set; }
        string Title { get; set; }
        int TotalCount { get; set; }
        int RentedCount { get; set; }
    }
}

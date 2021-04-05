namespace Microblink.Library.Services.Models.Dto.Interfaces
{
    public interface IBook
    {  
        int Id { get; set; }
        string Code { get; set; }
        int BookTitleId { get; set; }
    }
}

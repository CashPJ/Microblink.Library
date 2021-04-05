using Microblink.Library.Services.Models;
using Microblink.Library.Services.Models.Dto;
using Microblink.Library.Services.Models.Interfaces;
using Microblink.Library.Services.Repositories;
using System.Linq;

namespace Microblink.Library.Services.Interfaces
{
    public interface IDatabaseService
    {
        UserRepository Users { get; set; }
        IQueryable<IContactType> GetContactTypes();

        IQueryable<BookTitle> GetBookTitles();

        IQueryable<User> GetUsers();

        IQueryable<IContact> GetUserContacts(int userId);

        IQueryable<Rent> GetRents();

        IQueryable<Book> GetBooks();
    }
}

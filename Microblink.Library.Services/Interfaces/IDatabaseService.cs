using Microblink.Library.Services.Models;
using Microblink.Library.Services.Models.Interfaces;
using Microblink.Library.Services.Repositories;
using System.Linq;

namespace Microblink.Library.Services.Interfaces
{
    public interface IDatabaseService
    {
        UserRepository Users { get; set; }
        IQueryable<IContactType> GetContactTypeCodebook();

        IQueryable<IBookTitle> GetBookTitles();
    }
}

using Microblink.Library.Services.Contexts;
using Microblink.Library.Services.Models.Dto;

namespace Microblink.Library.Services.Repositories
{
    public class BookRepository : RepositoryBase<Book>
    {
        private readonly DatabaseContext _dbContext;
        public BookRepository(DatabaseContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}

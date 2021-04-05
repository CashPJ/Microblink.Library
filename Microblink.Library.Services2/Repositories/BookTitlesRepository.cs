using Microblink.Library.Services.Contexts;
using Microblink.Library.Services.Models.Dto;

namespace Microblink.Library.Services.Repositories
{
    public class BookTitlesRepository : RepositoryBase<BookTitle>
    {
        private readonly DatabaseContext _dbContext;
        public BookTitlesRepository(DatabaseContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}

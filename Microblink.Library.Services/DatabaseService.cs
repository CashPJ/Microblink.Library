using Microblink.Library.Services.Contexts;
using Microblink.Library.Services.Interfaces;
using Microblink.Library.Services.Models;
using Microblink.Library.Services.Models.Interfaces;
using Microblink.Library.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Microblink.Library.Services
{
    public class DatabaseService : IDatabaseService
    {

        private readonly DatabaseContext DbContext;

        private UserRepository _users;
        public UserRepository Users
        {
            get
            {
                if (_users == null)
                    _users = new UserRepository(DbContext);

                return _users;
            }
            set
            {
                _users = value;
            }
        }

        public DatabaseService(DatabaseContext dbContext)
        {
            DbContext = dbContext;
        }

        public DatabaseService(string connectionString)
        {
            DbContext = new DatabaseContext(connectionString);
        }

        public IQueryable<IContactType> GetContactTypeCodebook()
        {
            return DbContext.Set<ContactTypeDto>().AsNoTracking();
        }

        public IQueryable<IBookTitle> GetBookTitles()
        {
            return DbContext.Set<BookTitleDto>().AsNoTracking();
        }

    }
}

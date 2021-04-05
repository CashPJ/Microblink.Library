using Microblink.Library.Services.Contexts;
using Microblink.Library.Services.Interfaces;
using Microblink.Library.Services.Models;
using Microblink.Library.Services.Models.Dto;
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

        public IQueryable<IContactType> GetContactTypes()
        {
            return DbContext.Set<ContactType>().AsNoTracking();
        }

        public IQueryable<Book> GetBooks()
        {
            return DbContext.Set<Book>().AsNoTracking();
        }

        public IQueryable<BookTitle> GetBookTitles()
        {
            return DbContext.Set<BookTitle>().AsNoTracking();
        }

        public IQueryable<User> GetUsers()
        {
            return Users.GetAll();
        }

        public IQueryable<Rent> GetRents()
        {
            return DbContext.Set<Rent>().AsNoTracking();
        }

        public IQueryable<IContact> GetUserContacts(int userId)
        {
            return DbContext.Set<Contact>().AsNoTracking().Where((System.Linq.Expressions.Expression<System.Func<Contact, bool>>)(c => (bool)(c.UserId == userId)));//.OrderBy<ContactDto,>(o => o.ContactType.SortOrder);
        }


    }
}

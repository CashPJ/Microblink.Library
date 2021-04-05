using Microblink.Library.Services.Contexts;
using Microblink.Library.Services.Models.Dto;
using Microblink.Library.Services.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microblink.Library.Services.Repositories
{
    public class UserRepository : RepositoryBase<User>
    {
        private readonly DatabaseContext _dbContext;
        public UserRepository(DatabaseContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /*
        private IQueryable<Contact> GetContacts(int userId)
        {
            return _dbContext.Contacts
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.ContactType.SortOrder)
                .Select(x =>  new Contact() { Id = x.Id, ContactTypeId = x.ContactTypeId, ContactTypeName = x.ContactType.Name, UserId = x.UserId, Value = x.Value });
        }
        */
    }
}

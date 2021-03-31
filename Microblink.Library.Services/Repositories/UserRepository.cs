using Microblink.Library.Services.Contexts;
using Microblink.Library.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microblink.Library.Services.Repositories
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }
    }
}

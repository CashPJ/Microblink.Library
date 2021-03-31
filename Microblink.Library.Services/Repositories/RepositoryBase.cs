using Microblink.Library.Services.Contexts;
using Microblink.Library.Services.Models.Interfaces;
using Microblink.Library.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Microblink.Library.Services.Repositories
{
    /// <summary>
    /// Generic repository CRUD implementation 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IEntity
    {
        private readonly DatabaseContext Context;

        public RepositoryBase(DatabaseContext context)
        {
            Context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }

        public async Task<TEntity> Get(int id)
        {
            return await Context.Set<TEntity>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Create(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Update(int id, TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await Get(id);
            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
}


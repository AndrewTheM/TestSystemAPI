using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TestSystem.API.Repositories.Interfaces;

namespace TestSystem.API.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _set;

        public Repository(ApplicationContext context) => _set = context.Set<T>();

        public Task<IQueryable<T>> GetAll() => Task.Run(() => _set.AsQueryable());

        public async Task<T> Get(int id) => await _set.FindAsync(id);

        public async Task Add(T item) => await _set.AddAsync(item);

        public Task Remove(T item) => Task.Run(() => _set.Remove(item));
    }
}

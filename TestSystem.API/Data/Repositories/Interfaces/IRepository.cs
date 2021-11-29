using System.Linq;
using System.Threading.Tasks;

namespace TestSystem.API.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAll();
        Task<T> Get(int id);
        Task Add(T item);
        Task Remove(T item);
    }
}

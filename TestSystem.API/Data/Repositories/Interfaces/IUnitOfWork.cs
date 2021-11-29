using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TestSystem.API.Core.Entities;

namespace TestSystem.API.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Test> Tests { get; }
        IRepository<Question> Questions { get; }
        IRepository<Answer> Answers { get; }
        IRepository<Attempt> Attempts { get; }

        UserManager<User> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        SignInManager<User> SignInManager { get; }
        
        Task ModifyStateAsync<T>(T entity) where T : class;
        Task SaveChangesAsync();
    }
}

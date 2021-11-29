using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TestSystem.API.Core.Entities;
using TestSystem.API.Repositories.Interfaces;

namespace TestSystem.API.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationContext _context;

        public UnitOfWork(ApplicationContext context,
                            IRepository<Test> tests,
                            IRepository<Question> questions,
                            IRepository<Answer> answers,
                            IRepository<Attempt> attempts,
                            UserManager<User> userManager,
                            RoleManager<IdentityRole> roleManager,
                            SignInManager<User> signInManager)
        {
            _context = context;
            Tests = tests;
            Questions = questions;
            Answers = answers;
            Attempts = attempts;
            UserManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;
        }

        public IRepository<Test> Tests { get; }
        public IRepository<Question> Questions { get; }
        public IRepository<Answer> Answers { get; }
        public IRepository<Attempt> Attempts { get; }

        public UserManager<User> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public SignInManager<User> SignInManager { get; }

        public void Dispose() => _context.Dispose();

        public Task ModifyStateAsync<T>(T entity) where T : class => Task.Run(() => ModifyState(entity));

        private void ModifyState<T>(T entity) where T : class => _context.Entry(entity).State = EntityState.Modified;

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using TestSystem.API.Core.Entities;
using TestSystem.API.Repositories.Interfaces;

namespace TestSystem.API.Helpers
{
    public class UserInitializer
    {
        private const string AdminName = "Admin";

        public async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetService<IUnitOfWork>();
            var adminData = serviceProvider.GetService<IOptions<DefaultAdminOptions>>().Value;

            if (adminData == null)
                return;

            var currentAdmin = await uow.UserManager.FindByNameAsync(adminData.Username);
            if (currentAdmin != null)
                return;

            var adminRole = await uow.RoleManager.FindByNameAsync(AdminName);
            if (adminRole == null)
            {
                adminRole = new IdentityRole { Name = AdminName, NormalizedName = AdminName.ToUpper() };

                var roleResult = await uow.RoleManager.CreateAsync(adminRole);
                if (!roleResult.Succeeded)
                    return;
            }

            var adminUser = new User
            {
                UserName = adminData.Username,
                Email = adminData.Email,
                FirstName = adminData.FirstName,
                LastName = adminData.LastName
            };

            var userResult = await uow.UserManager.CreateAsync(adminUser, adminData.Password);
            if (!userResult.Succeeded)
                return;

            var adminResult = await uow.UserManager.AddToRoleAsync(adminUser, AdminName);
            if (!adminResult.Succeeded)
                return;

            await uow.SaveChangesAsync();
        }
    }
}

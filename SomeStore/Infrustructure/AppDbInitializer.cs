using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SomeStore.Infrustructure.Constants;
using SomeStore.Infrustructure.DbEntities;
using SomeStore.Models;

namespace SomeStore.Infrustructure
{
    public class AppDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;

        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);

            var passwordValidator = new PasswordValidator { RequiredLength = AppConstants.PasswordMinLength };

            using (_roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)))
            using (_userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context)) { PasswordValidator = passwordValidator })
            {
                TryCreateRole(AppRoles.Admin);
                TryCreateRole(AppRoles.User);
                TryCreateUser("admin@test.com", "123", AppRoles.Admin);
                TryCreateUser("user@test.com", "123", AppRoles.User);
            }

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Bread",
                    Description = "Tasty bread",
                    Price = 1,
                    ImageUrl = "http://www.stmarksmilwaukee.org/wp-content/uploads/2016/07/bread.jpg"
                },
                new Product
                {
                    Name = "Table",
                    Description = "Big dark table for all family",
                    Price = 299,
                    ImageUrl = "http://fernandgrey.com/media/catalog/product/b/r/brent-1.5m-dining-table-black-opt.jpg"
                }
            };

            context.Products.AddRange(products);
        }

        private void TryCreateRole(string roleName)
        {
            if (!_roleManager.RoleExists(roleName))
            {
                var role = new IdentityRole
                {
                    Name = roleName
                };
                _roleManager.Create(role);
            }
        }

        private void TryCreateUser(string email, string password, string role)
        {
            if (_userManager.FindByName(email) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                var chkUser = _userManager.Create(user, password);
                if (chkUser.Succeeded && _roleManager.RoleExists(role))
                {
                    _userManager.AddToRole(user.Id, role);
                }
            }
        }
    }
}

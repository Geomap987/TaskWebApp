using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskWebApp.DbStuff.Models;

namespace TaskWebApp.DbStuff.Seed
{
    public static class DemoUserSeed
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<WebDbContext>();
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

            await db.Database.MigrateAsync();

            await EnsureUserAsync(
                db, hasher,
                name: "Bob Johnson",
                email: "admin@example.com",
                login: "admin_demo",
                password: "Admin135!Demo",
                isAdmin: true);

            await EnsureUserAsync(
                db, hasher,
                name: "Mary Johnson",
                email: "maryjohnson@example.com",
                login: "Mary_demo",
                password: "Mary135!Demo",
                isAdmin: false);

            await EnsureUserAsync(
                db, hasher,
                name: "Alex Johnson",
                email: "alexjohnson@example.com",
                login: "Alex",
                password: "Alex135!Demo",
                isAdmin: false);

            await db.SaveChangesAsync();
        }

        private static async Task EnsureUserAsync(
            WebDbContext db,
            IPasswordHasher<User> hasher,
            string name,
            string email,
            string login,
            string password,
            bool isAdmin)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Login == login);

            if (user == null)
            {
                user = new User
                {
                    Name = name,
                    Email = email,
                    Login = login,
                    Role = isAdmin ? "admin" : ""
                };

                user.PasswordHash = hasher.HashPassword(user, password);
                db.Users.Add(user);
            }
        }
    }

}

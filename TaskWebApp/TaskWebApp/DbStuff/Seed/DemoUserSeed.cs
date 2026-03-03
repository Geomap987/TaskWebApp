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
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

            await db.Database.MigrateAsync();

            var adminPwd = GetRequiredSeedPassword(config, env, "admin_demo");
            var maryPwd = GetRequiredSeedPassword(config, env, "Mary_demo");
            var alexPwd = GetRequiredSeedPassword(config, env, "Alex_demo");

            await EnsureUserAsync(
                db, hasher,
                name: "Bob Johnson",
                email: "admin@example.com",
                login: "admin_demo",
                password: adminPwd,
                isAdmin: true);

            await EnsureUserAsync(
                db, hasher,
                name: "Mary Johnson",
                email: "maryjohnson@example.com",
                login: "Mary_demo",
                password: maryPwd,
                isAdmin: false);

            await EnsureUserAsync(
                db, hasher,
                name: "Alex Johnson",
                email: "alexjohnson@example.com",
                login: "Alex_demo",
                password: alexPwd,
                isAdmin: false);

            await db.SaveChangesAsync();
        }

        private static string GetRequiredSeedPassword(
            IConfiguration config,
            IHostEnvironment env,
             string login)
        {
            var key = $"DemoSeed:Users:{login}:Password";
            var pwd = config[key];

            if (!string.IsNullOrWhiteSpace(pwd))
                return pwd;

            var msg =
                $"Missing demo password for '{login}'. Set '{key}' via user-secrets or environment variables.";

            if (env.IsDevelopment())
                throw new InvalidOperationException(msg);

            throw new InvalidOperationException(msg);
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

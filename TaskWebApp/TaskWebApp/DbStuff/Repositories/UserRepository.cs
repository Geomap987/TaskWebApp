using Microsoft.EntityFrameworkCore;
using TaskWebApp.DbStuff.Models;
using TaskWebApp.DbStuff.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace TaskWebApp.DbStuff.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserRepository(WebDbContext context, IPasswordHasher<User> passwordHasher) : base(context)
        {
            _passwordHasher = passwordHasher;
        }

        public List<UserBasicInfo> GetAllUsersBasicInfo()
        {
            return _entyties
                .Select(user => new UserBasicInfo
                {
                    Id = user.Id,
                    Name = user.Login
                })
                .ToList();
        }

        public List<User> GetUsersByIds(List<int> ids)
        {
            return _entyties.Where(user => ids.Contains(user.Id)).ToList();
        }
        public bool AnyUserWithName(string name)
        {
            return _entyties.Any(x => x.Login == name);
        }

        public bool AnyUserWithEmail(string email)
        {
            return _entyties.Any(x => x.Email == email);
        }

        public User? GetUserByLoginAndPassword(string login, string password)
        {
            var user = _entyties.FirstOrDefault(u => u.Login == login);
            if (user == null) return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, password);

            return result == PasswordVerificationResult.Success
                ? user
                : null;
        }


        public User GetUserByEmail(string email)
        {
            var user = _entyties.FirstOrDefault(x => x.Email == email);
            return user;
        }

        public void SwitchLocal(int userId, string locale)
        {
            var user = GetById(userId);
            user.PreferLocale = locale;
            _context.SaveChanges();
        }
    }
}

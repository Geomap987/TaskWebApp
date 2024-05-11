using Microsoft.EntityFrameworkCore;
using TaskWebApp.DbStuff.Models;

namespace TaskWebApp.DbStuff.Repositories
{
    public class UserRepository : BaseRepository<User>
    {

        public UserRepository(WebDbContext context) : base(context)
        {
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
            return _entyties
                .FirstOrDefault(user => user.Login == login && user.Password!.Equals(password));
        }

        //public async Task<User?> GetUserByLoginAndPasswordAsync(string login, string password)
        //{
        //    return await _entyties
        //        .FirstOrDefaultAsync(user => user.Login == login && user.Password!.Equals(password));
        //}

        public User GetUserByEmail(string email)
        {
            var user = _entyties.FirstOrDefault(x => x.Email == email);
            return user;
        }


        //public void UpdateUser(User oldUser, UserViewModel updateUser)
        //{
        //    _userEditHelper.EditUser(oldUser, updateUser);
        //    _context.SaveChanges();
        //}

        //public async Task<bool> UpdateUserAsync(User oldUser, UserViewModel updateUser)
        //{
        //    _userEditHelper.EditUser(oldUser, updateUser);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        public void SwitchLocal(int userId, string locale)
        {
            var user = GetById(userId);
            user.PreferLocale = locale;
            _context.SaveChanges();
        }
    }
}

using System.Linq;
using BuildFlowApp.Models;
using BuildFlowApp.Data;

namespace BuildFlowApp.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _db;

        public AuthService(ApplicationDbContext db)
        {
            _db = db;
            _db.Database.EnsureCreated();
        }

        public bool Register(string username, string email, string password)
        {
            if (_db.Users.Any(u => u.Email == email)) return false;
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };
            _db.Users.Add(user);
            _db.SaveChanges();
            return true;
        }

        public User? Login(string email, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;
            return user;
        }
    }
}

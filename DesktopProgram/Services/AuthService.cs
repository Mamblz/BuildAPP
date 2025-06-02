using DesktopProgram.Data;
using DesktopProgram.Models;
using System.Text;
using System.Security.Cryptography;

namespace DesktopProgram.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService()
        {
            _context = new ApplicationDbContext();
            _context.Database.EnsureCreated();
        }

        public bool Register(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
                return false;

            if (_context.Users.Any(u => u.Username == username || u.Email == email))
                return false;

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return true;
        }

        public User Login(string usernameOrEmail, string password)
        {
            if (string.IsNullOrWhiteSpace(usernameOrEmail) || string.IsNullOrWhiteSpace(password))
                return null;

            var user = _context.Users.FirstOrDefault(u =>
                u.Username == usernameOrEmail || u.Email == usernameOrEmail);

            if (user == null)
                return null;

            return user.PasswordHash == HashPassword(password) ? user : null;
        }

        public string GetHashedPassword(string password)
        {
            return HashPassword(password);
        }

        public void UpdateUserPassword(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.PasswordHash = user.PasswordHash;
                _context.SaveChanges();
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}

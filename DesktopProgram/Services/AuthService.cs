using DesktopProgram.Data;
using DesktopProgram.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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

        public bool Login(string usernameOrEmail, string password)
        {
            if (string.IsNullOrWhiteSpace(usernameOrEmail) || string.IsNullOrWhiteSpace(password))
                return false;

            var user = _context.Users.FirstOrDefault(u =>
                u.Username == usernameOrEmail || u.Email == usernameOrEmail);

            if (user == null)
                return false;

            return user.PasswordHash == HashPassword(password);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
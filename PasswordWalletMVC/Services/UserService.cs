using PasswordWalletMVC.HashGenerators;
using PasswordWalletMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace PasswordWalletMVC.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        bool Register(User user, string submit);
        IEnumerable<User> Unregister(User user);
        int Login(User user);
        void ChangePassword(User user, int id);
    }

    public class UserService : IUserService
    {
        private readonly WalletDbContext _walletDbContext;

        public UserService(WalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public IEnumerable<User> GetUsers()
        {
            var users = _walletDbContext.Users.ToList();

            return users;
        }

        public bool Register(User user, string submit)
        {
            var exist = _walletDbContext.Users.FirstOrDefault(u => u.Login == user.Login);
            if (exist != null)
            {
                return false;
            }
            user.Salt = GenerateSalt();

            switch (submit)
            {
                case "hmac":
                    user.PasswordHash = HashGenerator.GenerateHmac(user.PasswordHash, user.Salt);
                    break;


                case "sha":
                    user.PasswordHash = HashGenerator.GenerateSha(user.PasswordHash, user.Salt);
                    break;


                default:
                    break;
            }
            _walletDbContext.Users.Add(user);
            _walletDbContext.SaveChanges();
            return true;
        }

        public IEnumerable<User> Unregister(User user)
        {
            _walletDbContext.Remove(user);
            _walletDbContext.SaveChanges();

            var users = _walletDbContext.Users.ToList();

            return users;
        }

        public int Login(User user)
        {

            var userSalt = _walletDbContext.Users.Where(u => u.Login == user.Login).Select(u => u.Salt).FirstOrDefault();
            var hmac = HashGenerator.GenerateHmac(user.PasswordHash, userSalt);
            var sha = HashGenerator.GenerateSha(user.PasswordHash, userSalt);
            var userDb = _walletDbContext.Users.FirstOrDefault(u => u.Login == user.Login && (u.PasswordHash == hmac || u.PasswordHash == sha));

            if (userDb is null)
            {
                return -1;
            }

            return userDb.UserId;
        }

        public void ChangePassword(User user, int id)
        {
            var userDb = _walletDbContext.Users.FirstOrDefault(u => u.UserId == id);          

            userDb.PasswordHash = HashGenerator.GenerateHmac(user.PasswordHash, userDb.Salt);
            _walletDbContext.SaveChanges();
        }
        
        private string GenerateSalt()
        {
            string salt = Guid.NewGuid().ToString();
            return salt;
        }
    }
}
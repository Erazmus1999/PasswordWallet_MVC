using Microsoft.EntityFrameworkCore;
using PasswordWalletMVC.HashGenerators;
using PasswordWalletMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace PasswordWalletMVC.Services
{
    public interface IPasswordService
    {
        IEnumerable<Password> GetPasswords(int id);
        void AddingPassword(Password password);
        void HashingPassword(int id);
        void UnhashingPassword(int id);

    }

    public class PasswordService : IPasswordService
    {
        private readonly WalletDbContext _walletDbContext;

        public PasswordService(WalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public void AddingPassword(Password password)
        {
            _walletDbContext.Passwords.Add(password);
            _walletDbContext.SaveChanges();
        }

        public IEnumerable<Password> GetPasswords(int id)
        {
            var passwords = _walletDbContext.Passwords.Where(u => u.UserId == id).ToList();

            return passwords;
        }

        public void HashingPassword(int id)
        {
            var password = _walletDbContext.Passwords.FirstOrDefault(p => p.PasswordId == id);

            string key = "b14ca5898a4e4133bbce2ea2315a1916";
            password.PasswordName = SymetricalEncription.EncryptString(key, password.PasswordName);

            
            _walletDbContext.Passwords.Update(password);
            _walletDbContext.SaveChanges();          
        }

        public void UnhashingPassword(int id)
        {
            var password = _walletDbContext.Passwords.FirstOrDefault(p => p.PasswordId == id);

            string key = "b14ca5898a4e4133bbce2ea2315a1916";
            password.PasswordName = SymetricalEncription.DecryptString(key, password.PasswordName);

            _walletDbContext.Passwords.Update(password);
            _walletDbContext.SaveChanges();
        }
    }
}
using System;
using System.Threading.Tasks;
using MakeBuddies.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MakeBuddies.API.Data
{
    public class AuthRepo : IAuthRepo
    {
        private readonly DataContext context;
        public AuthRepo(DataContext context)
        {
            this.context = context;
        }
        public async Task<User> LoginAsync(string userName, string password)
        {
           var user = await context.Users.FirstOrDefaultAsync(x => x.Username == userName);

           if(user == null)
            return null;
        
           if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
           return null;

           return user;        

        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for(int i = 0; i< hash.Length; i++)
                {
                    if(hash[i] != passwordHash[i])
                    return false;
                }

                return true;

            }
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            generatePasswordHash(password,out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await context.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        private void generatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA256())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExistAsync(string userName)
        {
            if(await context.Users.AnyAsync(x => x.Username == userName))
                return true;
            
            return false;
        }
    }
}
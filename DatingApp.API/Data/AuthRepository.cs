using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private DataContext _dataContext;
        public AuthRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

         public async Task<User> Register( User user, string password){
             byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password,out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _dataContext.Users.AddAsync(user);
            _dataContext.SaveChanges();
            return user;

         }
         private void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt){
             using(var hashVal = new System.Security.Cryptography.HMACSHA512()){
                 passwordSalt = hashVal.Key;
                 passwordHash = hashVal.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
             }

         }
         public async Task<User> Login(string username, string password){
             var user = await _dataContext.Users.Include(p => p.Photos).FirstOrDefaultAsync(x=>x.UserName == username);
             if(user == null){
                 return null;
             }
            if( !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)){
                return null;
            }
            return user;
         }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
             using(var hashVal = new System.Security.Cryptography.HMACSHA512(passwordSalt)){
                 var computedHash = hashVal.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                 for (int i = 0; i < computedHash.Length; i++)
                 {
                     if(computedHash[i] != passwordHash[i]){ return false; };
                 }
                 return true;
             }
        }

        public async Task<bool> UserExists(string username){
            return await _dataContext.Users.AnyAsync(x=>x.UserName == username);
         }


    }
}
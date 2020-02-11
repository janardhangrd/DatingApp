using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public class Seed
    {
         private static void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt){
             using(var hashVal = new System.Security.Cryptography.HMACSHA512()){
                 passwordSalt = hashVal.Key;
                 passwordHash = hashVal.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
             }
         }

        public static void SeedUsers(DataContext context) { 
            if(!context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users)
                {
                    byte[] passwordhash, passwordSalt;
                    CreatePasswordHash("password",out passwordhash, out passwordSalt);

                    user.PasswordHash = passwordhash;
                    user.PasswordSalt = passwordSalt;
                    user.UserName = user.UserName.ToLower();
                    context.Users.Add(user);
                }
                context.SaveChanges();
            }
        }
    }
}
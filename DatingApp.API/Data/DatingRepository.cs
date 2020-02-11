using DatingApp.API.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext context;
        public DatingRepository(DataContext context)
        {
            this.context = context;
        }

        public void Add<T>(T entity) where T:class {
            context.Add(entity);
        }

        public void Delete<T>(T entity) where T:class {
            context.Remove(entity);
        }

        public async Task<bool> SaveAll(){
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<User>> GetUsers(){
            var userList = await context.Users.Include(p=>p.Photos).ToListAsync();
            return userList;
        }
 
        public async Task<User> GetUser(int id){ 
            var user = await context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u=>u.Id == id);
            return user;
        }     
    }
}
using DatingApp.API.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await context.Photos.FirstOrDefaultAsync(p =>p.Id == id);
            return photo;
        }

        public async Task<Photo> GetMainPhotoForUser(int userid)
        {
            return await context.Photos.Where(p=>p. UserId == userid).FirstOrDefaultAsync(p=>p.IsMain);
        }
    }
}
using PropTrade.Api.DataAccess;
using PropTrade.Api.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace PropTrade.Api.Repositories
{
    public class UsersRepository : IUsersRepository
    {        
        public UsersRepository()
        {
            this.dbContext = new PropTradeDbContext();
        }

        public IQueryable<User> GetUsersQuery()
        {
            return this.dbContext.Users.AsQueryable();
        }

        public async Task<User> GetUser(Guid userId)
        {
            return await this.dbContext.Users.FindAsync(userId);
        }

        public async Task<User> GetUser(string username)
        {
            return await this.dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IList<User>> GetUsers(int? skip, int? take)
        {
            int skipValue = skip.HasValue ? skip.Value : 0;
            int takeValue = take.HasValue ? take.Value : 50;
            var query = this.dbContext.Users
                .OrderByDescending(u => u.Created)
                .Skip(skipValue)
                .Take(takeValue);

            return await query.ToListAsync();
        }

        public async Task<bool> CreateUser(User newUser)
        {
            this.dbContext.Users.Add(newUser);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateUser(User modifiedUser)
        {
            var existingUser = await this.dbContext.Users.FindAsync(modifiedUser.Id);

            if (existingUser == null)
            {
                return false;
            }

            // Explicitly map the properties which can be changed to avoid changing some of the unchangable ones (ex: Username)
            existingUser.Profile.FirstName = modifiedUser.Profile.FirstName;
            existingUser.Profile.LastName = modifiedUser.Profile.LastName;            
            existingUser.Profile.LastModified = modifiedUser.Profile.LastModified;
            existingUser.Profile.CurrentTradeRole = modifiedUser.Profile.CurrentTradeRole;
            existingUser.Profile.Address = modifiedUser.Profile.Address;

            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<User> DeleteUser(Guid userId)
        {
            var existingUser = await this.dbContext.Users.FindAsync(userId);
            this.dbContext.Users.Remove(existingUser);
            await this.dbContext.SaveChangesAsync();

            return existingUser;
        }

        private PropTradeDbContext dbContext;
    }
}
using PropTrade.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropTrade.Api.Repositories
{
    public interface IUsersRepository
    {
        IQueryable<User> GetUsersQuery();

        Task<User> GetUser(Guid userId);

        Task<User> GetUser(string username);

        Task<IList<User>> GetUsers(int? skip, int? take);

        Task<bool> CreateUser(User newUser);

        Task<bool> UpdateUser(User modifiedUser);

        Task<User> DeleteUser(Guid userId);
    }
}
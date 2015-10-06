using PropTrade.Api.Entities;
using PropTrade.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropTrade.Api.Tests.Mocks
{
    public class UsersRepositoryMock : IUsersRepository
    {
        public UsersRepositoryMock(User userMock)
        {
            this.userMock = userMock;
        }

        public IQueryable<Entities.User> GetUsersQuery()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(Guid userId)
        {
            return Task.FromResult(this.userMock);
        }

        public Task<Entities.User> GetUser(string username)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Entities.User>> GetUsers(int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateUser(Entities.User newUser)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUser(Entities.User modifiedUser)
        {
            throw new NotImplementedException();
        }

        public Task<Entities.User> DeleteUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        private User userMock;
    }
}

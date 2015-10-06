using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropTrade.Api.Entities;
using System.Collections.Generic;
using PropTrade.Api.Controllers;
using PropTrade.Api.Tests.Mocks;
using PropTrade.Api.Repositories;
using PropTrade.Api.Dtos;
using System.Web.Http.Results;

namespace PropTrade.Api.Tests
{
    [TestClass]
    public class UsersControllerTests
    {
        [TestMethod]
        public void GetUser_ReturnsUserDto()
        {
            Guid IdMock = Guid.NewGuid();
            var userMock = new User()
            {
                Id = IdMock,
                Username = "test username",
                Profile = new UserProfile()
                {
                    Id = IdMock,
                    FirstName = "test firstname",
                    LastName = "test lastname",
                    CurrentTradeRole = TradeRole.Seller,
                    Address = new Address() { Country = "UK" }
                }
            };
            
            IUsersRepository usersRepository = new UsersRepositoryMock(userMock);
            UsersController controller = new UsersController(usersRepository);

            var actionResult = controller.GetUser(IdMock).Result;
            var response = actionResult as OkNegotiatedContentResult<UserDto>;

            Assert.IsNotNull(response.Content);
            Assert.AreEqual<Guid>(IdMock, response.Content.Id);
            Assert.AreEqual<string>(userMock.Username, response.Content.Username);
            Assert.AreEqual<string>(userMock.Profile.FirstName, response.Content.FirstName);
            Assert.AreEqual<string>(userMock.Profile.LastName, response.Content.LastName);
            Assert.AreEqual<string>(userMock.Profile.CurrentTradeRole.ToString(), response.Content.CurrentTradeRole.ToString());
            Assert.AreEqual<string>(userMock.Profile.Address.Country, response.Content.Address.Country);
        }
    }
}

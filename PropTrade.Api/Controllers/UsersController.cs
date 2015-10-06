using PropTrade.Api.Authentication;
using PropTrade.Api.Dtos;
using PropTrade.Api.Entities;
using PropTrade.Api.Helpers;
using PropTrade.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PropTrade.Api.Controllers
{
    [CustomBasicAuthentication]
    [Authorize]
    public class UsersController : ApiController
    {
        public UsersController()
        {
            this.usersRepository = new UsersRepository();
        }

        public UsersController(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        // GET: api/v1/users
        [ResponseType(typeof(IEnumerable<UserDto>))]
        public async Task<IHttpActionResult> GetUsers(bool includeRelatedData = false, int skip = 0, int take = 50)
        {
            var dbResult = await this.usersRepository.GetUsers(skip, take);
            IEnumerable<UserDto> result;

            if (includeRelatedData)
            {
                result =  dbResult.Select(u => new UserDto()
                {
                    Id = u.Id,
                    Username = u.Username,
                    Created = u.Created,
                    FirstName = u.Profile.FirstName,
                    LastName = u.Profile.LastName,
                    LastModified = u.Profile.LastModified,
                    CurrentTradeRole = Mapper.MapTradeRoleEntityToDto(u.Profile.CurrentTradeRole),
                    Address = Mapper.MapAddressEntityToDto(u.Profile.Address),
                    Offers = u.Profile.Offers.Select(o => Mapper.MapOfferEntityToDto(o, false)),
                    Properties = u.Profile.Properties.Select(p => Mapper.MapPropertyEntityToDto(p,false))
                });
            }
            else
            {
                result = dbResult.Select(u => new UserDto()
                {
                    Id = u.Id,
                    Username = u.Username,
                    Created = u.Created,
                    FirstName = u.Profile.FirstName,
                    LastName = u.Profile.LastName,
                    LastModified = u.Profile.LastModified,
                    CurrentTradeRole = Mapper.MapTradeRoleEntityToDto(u.Profile.CurrentTradeRole)
                });                
            }

            return Ok(result);
        }

        // GET: api/v1/users/5
        [ResponseType(typeof(UserDto))]
        public async Task<IHttpActionResult> GetUser(Guid id)
        {
            var dbResult = await this.usersRepository.GetUser(id);

            return Ok(new UserDto()
            {                
                Id = dbResult.Profile.Id,
                FirstName = dbResult.Profile.FirstName,
                LastName = dbResult.Profile.LastName,
                Created = dbResult.Profile.Created,
                LastModified = dbResult.Profile.LastModified,
                Address = Mapper.MapAddressEntityToDto(dbResult.Profile.Address),
                CurrentTradeRole = Mapper.MapTradeRoleEntityToDto(dbResult.Profile.CurrentTradeRole),
                Offers = dbResult.Profile.Offers.Select(o => Mapper.MapOfferEntityToDto(o, false)),
                Properties = dbResult.Profile.Properties.Select(p => Mapper.MapPropertyEntityToDto(p, false))
            });
        }

        // GET: api/v1/users/5
        [ResponseType(typeof(UserDto))]
        public async Task<IHttpActionResult> GetUser(string username)
        {
            var dbResult = await this.usersRepository.GetUser(username);

            return Ok(new UserDto()
            {
                Id = dbResult.Id,
                Username = dbResult.Username,
                Created = dbResult.Created,
                FirstName = dbResult.Profile.FirstName,
                LastName = dbResult.Profile.LastName,
                LastModified = dbResult.Profile.LastModified,
                CurrentTradeRole = Mapper.MapTradeRoleEntityToDto(dbResult.Profile.CurrentTradeRole),
                Address = Mapper.MapAddressEntityToDto(dbResult.Profile.Address),
                Offers = dbResult.Profile.Offers.Select(o => Mapper.MapOfferEntityToDto(o, false)).ToList(),
                Properties = dbResult.Profile.Properties.Select(p => Mapper.MapPropertyEntityToDto(p, false)).ToList()
            });
        }

        // POST: api/v1/users
        [ResponseType(typeof(UserDto))]
        [AllowAnonymous]
        public async Task<IHttpActionResult> PostUser(UserDto newUser)
        {
            Guid userId = Guid.NewGuid();
            DateTime dateCreated = DateTime.UtcNow;

            var user = new User()
            {
                Id = userId,
                Created = dateCreated,
                Username = newUser.Username,
                Password = HashHelper.GetHash(newUser.Password, HashType.MD5),
                Profile = new UserProfile()
                {
                    Id = Guid.NewGuid(),
                    Created = dateCreated,
                    LastModified = dateCreated,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    CurrentTradeRole = Mapper.MapTradeRoleDtoToEntity(newUser.CurrentTradeRole)
                }
            };

            await this.usersRepository.CreateUser(user);
            newUser.Id = userId;

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, newUser);
        }


        // PUT: api/v1/users
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(UserDto modifiedUser)
        {
            if (modifiedUser.Address == null)
            {
                modifiedUser.Address = new AddressDto();
            }

            await this.usersRepository.UpdateUser(new User()
            {
                Id = modifiedUser.Id,

                Profile = new UserProfile()
                {
                    LastModified = DateTime.UtcNow,
                    FirstName = modifiedUser.FirstName,
                    LastName = modifiedUser.LastName,
                    CurrentTradeRole = Mapper.MapTradeRoleDtoToEntity(modifiedUser.CurrentTradeRole),
                    Address = Mapper.MapAddressDtoToEntity(modifiedUser.Address)
                }
            });

            return StatusCode(HttpStatusCode.NoContent);;
        }

        // DELETE: api/v1/users
        [ResponseType(typeof(UserDto))]
        public async Task<IHttpActionResult> DeleteUser(Guid id)
        {
            var deletedUser = await this.usersRepository.DeleteUser(id);

            if (deletedUser == null)
            {
                return NotFound();
            }

            return Ok(new UserDto()
            {
                Id = deletedUser.Id,
                Username = deletedUser.Username
            });
        }

        private IUsersRepository usersRepository;
    }
}

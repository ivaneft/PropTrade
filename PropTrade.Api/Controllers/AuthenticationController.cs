using PropTrade.Api.Authentication;
using PropTrade.Api.Dtos;
using PropTrade.Api.Helpers;
using PropTrade.Api.Repositories;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PropTrade.Api.Controllers
{
    public class AuthenticationController : ApiController
    {
        public AuthenticationController()
        {
            this.authenticationService = new AuthenticationService();
            this.usersRepository = new UsersRepository();
        }

        public AuthenticationController(IUsersRepository usersRepository, AuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
            this.usersRepository = usersRepository;
        }

        [Route("v1/authenticate")]
        [HttpGet]
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> Authenticate(string username, string password)
        {
            bool valid = await this.authenticationService.ValidateCredentials(username, password);

            if (valid)
            {
                string tokenPlainString = username + ':' + password;
                byte[] tokenBytes = Encoding.ASCII.GetBytes(tokenPlainString);
                string tokenBase64String = Convert.ToBase64String(tokenBytes);

                return Ok(tokenBase64String);
            }

            return Unauthorized();
        }

        [CustomBasicAuthentication]
        [Authorize]
        [Route("v1/identity")]
        [HttpGet]
        [ResponseType(typeof(UserDto))]
        public async Task<IHttpActionResult> GetIdentity()
        {
            var credentials = AuthenticationHelper.ExtractUserNameAndPassword(Request.Headers.Authorization.Parameter);
            var user = await this.usersRepository.GetUser(credentials.Item1);

            return Ok(new UserDto()
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.Profile.FirstName,
                    LastName = user.Profile.LastName,
                    CurrentTradeRole = Mapper.MapTradeRoleEntityToDto(user.Profile.CurrentTradeRole)
                });
        }


        private AuthenticationService authenticationService;
        private IUsersRepository usersRepository;
    }
}

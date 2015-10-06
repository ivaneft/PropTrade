using PropTrade.Api.Helpers;
using PropTrade.Api.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PropTrade.Api.Authentication
{
    public class AuthenticationService
    {
        public AuthenticationService()
        {
            this.usersRepository = new UsersRepository();
        }

        public AuthenticationService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<CustomPrincipal> Authenticate(string username, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await this.usersRepository.GetUser(username);

            if(user == null)
            {
                return new CustomPrincipal(new CustomIdentity(username, false));
            }

            bool passwordIsCorrect = HashHelper.CheckHash(password, user.Password, HashType.MD5);

            return new CustomPrincipal(new CustomIdentity(username, true));
        }

        public async Task<bool> ValidateCredentials(string username, string password)
        {
            var user = await this.usersRepository.GetUser(username);

            if (user == null) 
            {
                return false;
            }

            bool passwordIsCorrect = HashHelper.CheckHash(password, user.Password, HashType.MD5);

            return passwordIsCorrect;
        }

        public async Task ResetPassword(Guid userId, string newPassword)
        {
            var user = await this.usersRepository.GetUser(userId);
            user.Password = HashHelper.GetHash(newPassword, HashType.MD5);
            await this.usersRepository.UpdateUser(user);
        }

        private IUsersRepository usersRepository;
    }
}
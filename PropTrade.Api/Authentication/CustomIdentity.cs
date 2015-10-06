using System.Security.Principal;

namespace PropTrade.Api.Authentication
{
    public class CustomIdentity : IIdentity
    {
        public CustomIdentity(string name, bool isAuthenticated)
        {
            this.name = name;
            this.isAuthenticated = isAuthenticated;
        }

        public string AuthenticationType
        {
            get 
            {
                return "Basic";
            }
        }

        public bool IsAuthenticated
        {
            get 
            {
                return this.isAuthenticated;
            }
        }

        public string Name
        {
            get 
            {
                return this.name;
            }
        }

        private string name;
        private bool isAuthenticated;
    }
}
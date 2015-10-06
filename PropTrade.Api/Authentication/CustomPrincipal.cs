using System;
using System.Security.Principal;

namespace PropTrade.Api.Authentication
{
    public class CustomPrincipal : IPrincipal
    {
        public CustomPrincipal(IIdentity identity)
        {
            this.identity = identity;
        }

        public IIdentity Identity
        {
            get 
            { 
                return this.identity;
            }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        private IIdentity identity;
    }
}
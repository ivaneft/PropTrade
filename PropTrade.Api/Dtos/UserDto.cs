using System;
using System.Collections.Generic;

namespace PropTrade.Api.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastModified { get; set; }

        public TradeRoleDto CurrentTradeRole { get; set; }

        public AddressDto Address { get; set; }       

        public IEnumerable<PropertyDto> Properties { get; set; }

        public IEnumerable<OfferDto> Offers { get; set; }
    }
}
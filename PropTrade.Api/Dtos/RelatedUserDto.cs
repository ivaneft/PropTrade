using System;

namespace PropTrade.Api.Dtos
{
    public class RelatedUserDto
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public TradeRoleDto CurrentTradeRole { get; set; }
    }
}

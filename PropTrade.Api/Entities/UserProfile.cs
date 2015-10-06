using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace PropTrade.Api.Entities
{
    public class UserProfile
    {
        public UserProfile()
        {
            this.Address = new Address();
            this.Offers = new List<Offer>();
            this.Properties = new List<Property>();
        }
        
        [Key, ForeignKey("User")]
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Address Address { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastModified { get; set; }

        public TradeRole CurrentTradeRole { get; set; }

        [Required]
        public virtual User User { get; set; }

        public virtual ICollection<Property> Properties { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }
    }
}

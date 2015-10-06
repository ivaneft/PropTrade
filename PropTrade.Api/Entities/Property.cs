using PropTrade.Api.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace PropTrade.Api.Entities
{
    public class Property
    {
        public Property()
        {
            this.Location = new Address();
            this.Offers = new List<Offer>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public bool Sold { get; set; }

        public DateTime Created { get; set; }

        public Address Location { get; set; }

        [Required]
        [ForeignKey("Owner")]        
        public virtual Guid OwnerId { get; set; }
        
        public virtual UserProfile Owner { get; set; }
        
        public virtual ICollection<Offer> Offers { get; set; }
    }
}

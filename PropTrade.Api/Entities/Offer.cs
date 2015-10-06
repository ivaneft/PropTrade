using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropTrade.Api.Entities
{
    public class Offer
    {
        [Key]
        public Guid Id { get; set; }

        public double Value { get; set; }

        public DateTime Created { get; set; }
        
        [Required]
        [ForeignKey("Buyer")]
        public Guid BuyerId { get; set; }
        
        public virtual UserProfile Buyer { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace PropTrade.Api.Dtos
{
    public class OfferDto
    {
        public Guid Id { get; set; }

        public double Value { get; set; }

        public DateTime Created { get; set; }

        public Guid PropertyId { get; set; }

        [Required]
        public RelatedUserDto Buyer { get; set; }
    }
}
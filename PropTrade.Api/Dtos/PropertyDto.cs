using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PropTrade.Api.Dtos
{
    public class PropertyDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public bool Sold { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastModified { get; set; }

        public AddressDto Location { get; set; }

        public RelatedUserDto Owner { get; set; }

        public virtual IEnumerable<OfferDto> Offers { get; set; }
    }
}
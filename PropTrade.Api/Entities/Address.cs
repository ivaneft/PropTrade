using System.ComponentModel.DataAnnotations.Schema;

namespace PropTrade.Api.Entities
{
    [ComplexType]
    public class Address
    {
        public string Number { get; set; }

        public string Street { get; set; }

        public string CityTown { get; set; }

        public string ProvinceState { get; set; }

        public string Country { get; set; }
    }
}

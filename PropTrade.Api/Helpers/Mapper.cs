using PropTrade.Api.Dtos;
using PropTrade.Api.Entities;

namespace PropTrade.Api.Helpers
{
    public class Mapper
    {
        public static AddressDto MapAddressEntityToDto(Address address)
        {
            return new AddressDto()
            {
                Number = address.Number,
                Street = address.Street,
                CityTown = address.CityTown,
                ProvinceState = address.ProvinceState,
                Country = address.Country
            };
        }

        public static Address MapAddressDtoToEntity(AddressDto addressDto)
        {
            return new Address()
            {
                Number = addressDto.Number,
                Street = addressDto.Street,
                CityTown = addressDto.CityTown,
                ProvinceState = addressDto.ProvinceState,
                Country = addressDto.Country
            };
        }

        public static OfferDto MapOfferEntityToDto(Offer offer, bool mapBuyer = true)
        {
            return new OfferDto()
            {
                Id = offer.Id,
                Value = offer.Value,
                Created = offer.Created,
                Buyer = mapBuyer 
                    ? Mapper.MapUserAndProfileToRelatedUserDto(offer.Buyer.User, offer.Buyer)
                    : new RelatedUserDto() { UserId = offer.BuyerId }
            };
        }

        public static Offer MapOfferDtoToEntity(OfferDto offerDto)
        {
            return new Offer()
            {
                Id = offerDto.Id,
                Value = offerDto.Value,
                Created = offerDto.Created,
                BuyerId = offerDto.Buyer.UserId,
                //PropertyId = offerDto.PropertyId
            };
        }

        public static PropertyDto MapPropertyEntityToDto(Property property, bool mapOwner = true)
        {
            return new PropertyDto()
            {
                Id = property.Id,
                Name = property.Name,
                Price = property.Price,
                Description = property.Description,
                Sold = property.Sold,
                Created = property.Created,
                Location = Mapper.MapAddressEntityToDto(property.Location),
                Owner = mapOwner 
                    ? Mapper.MapUserAndProfileToRelatedUserDto(property.Owner.User, property.Owner) 
                    : new RelatedUserDto() { UserId = property.OwnerId }
            };
        }

        public static Property MapPropertyDtoToEntity(PropertyDto propertyDto)
        {
            return new Property()
            {
                Id = propertyDto.Id,
                Name = propertyDto.Name,
                Price = propertyDto.Price,
                Description = propertyDto.Description,
                Sold = propertyDto.Sold,
                OwnerId = propertyDto.Owner.UserId,
                Created = propertyDto.Created,
                Location = Mapper.MapAddressDtoToEntity(propertyDto.Location)
            };
        }

        public static RelatedUserDto MapUserAndProfileToRelatedUserDto(User user, UserProfile profile)
        {
            return new RelatedUserDto()
            {
                UserId = user.Id,
                Username = user.Username,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                CurrentTradeRole = Mapper.MapTradeRoleEntityToDto(profile.CurrentTradeRole)
            };
        }

        public static UserProfile MapRelatedUserDtoToProfile(RelatedUserDto relatedUserDto)
        {
            return new UserProfile()
            {
                Id = relatedUserDto.UserId,
                FirstName = relatedUserDto.FirstName,
                LastName = relatedUserDto.LastName,
                CurrentTradeRole = Mapper.MapTradeRoleDtoToEntity(relatedUserDto.CurrentTradeRole)
            };
        }

        public static TradeRoleDto MapTradeRoleEntityToDto(TradeRole tradeRole)
        {
            return (TradeRoleDto)tradeRole;
        }

        public static TradeRole MapTradeRoleDtoToEntity(TradeRoleDto tradeRoleDto)
        {
            return (TradeRole)tradeRoleDto;
        }
    }
}
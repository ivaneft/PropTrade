using PropTrade.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropTrade.Api.Repositories
{
    public interface IOffersRepository
    {
        IQueryable<Offer> GetOffersQuery();

        Task<Offer> GetOffer(Guid offerId);

        Task<IList<Offer>> GetOffers(int? skip, int? take);

        Task<bool> CreateOffer(Guid properyId, Offer newOffer);

        Task<bool> UpdateOffer(Offer modifiedOffer);

        Task<Offer> DeleteOffer(Guid offerId);
    }
}
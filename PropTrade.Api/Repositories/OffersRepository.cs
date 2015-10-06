using PropTrade.Api.DataAccess;
using PropTrade.Api.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace PropTrade.Api.Repositories
{
    public class OffersRepository : IOffersRepository
    {
        public OffersRepository()
        {
            this.dbContext = new PropTradeDbContext();
        }

        public IQueryable<Offer> GetOffersQuery()
        {
            return this.dbContext.Offers.AsQueryable();
        }

        public async Task<Offer> GetOffer(Guid offerId)
        {
            return await this.dbContext.Offers.FindAsync(offerId);
        }

        public async Task<IList<Offer>> GetOffers(int? skip, int? take)
        {
            int skipValue = skip.HasValue ? skip.Value : 0;
            int takeValue = take.HasValue ? take.Value : 50;
            var query = this.dbContext.Offers
                .OrderByDescending(p => p.Created)
                .Skip(skipValue)
                .Take(takeValue);

            return await query.ToListAsync();
        }

        public async Task<bool> CreateOffer(Guid propertyId, Offer newOffer)
        {
            var relatedProperty = await this.dbContext.Properties.FindAsync(propertyId);

            if (relatedProperty == null)
            {
                return false;
            }

            relatedProperty.Offers.Add(newOffer);
            // this.dbContext.Offers.Add(newOffer);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateOffer(Offer modifiedOffer)
        {
            var existingOffer = await this.dbContext.Offers.FindAsync(modifiedOffer.Id);

            if (existingOffer == null)
            {
                return false;
            }

            // Explicitly map the properties which can be changed to avoid changing some of the unchangable ones (ex: Username)
            existingOffer.Value = modifiedOffer.Value;
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<Offer> DeleteOffer(Guid offerId)
        {
            var existingOffer = await this.dbContext.Offers.FindAsync(offerId);

            if (existingOffer != null)
            {
                this.dbContext.Offers.Remove(existingOffer);
                await this.dbContext.SaveChangesAsync();
            }            

            return existingOffer;
        }

        private PropTradeDbContext dbContext;
    }
}
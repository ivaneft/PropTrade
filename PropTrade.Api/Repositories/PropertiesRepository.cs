using PropTrade.Api.DataAccess;
using PropTrade.Api.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace PropTrade.Api.Repositories
{
    public class PropertiesRepository : IPropertiesRepository
    {        
        public PropertiesRepository()
        {
            this.dbContext = new PropTradeDbContext();
        }

        public IQueryable<Entities.Property> GetPropertiesQuery()
        {
            return this.dbContext.Properties.AsQueryable();
        }

        public async Task<Property> GetProperty(Guid propertyId)
        {
            return await this.dbContext.Properties.FindAsync(propertyId);
        }

        public async Task<IList<Property>> GetProperties(int? skip, int? take)
        {
            int skipValue = skip.HasValue ? skip.Value : 0;
            int takeValue = take.HasValue ? take.Value : 50;
            var query = this.dbContext.Properties
                .OrderByDescending(p => p.Created)
                .Skip(skipValue)
                .Take(takeValue);

            return await query.ToListAsync();
        }

        public async Task<bool> CreateProperty(Property newProperty)
        {
            this.dbContext.Properties.Add(newProperty);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateProperty(Property modifiedProperty)
        {
            var existingProperty = await this.dbContext.Properties.FindAsync(modifiedProperty.Id);

            if (existingProperty == null)
            {
                return false;
            }

            // Explicitly map the properties which can be changed to avoid changing some of the unchangable ones (ex: Username)
            existingProperty.Name = modifiedProperty.Name;
            existingProperty.Description = modifiedProperty.Description;
            existingProperty.Price = modifiedProperty.Price;
            existingProperty.Sold = modifiedProperty.Sold;
            existingProperty.Location = modifiedProperty.Location;

            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Property> DeleteProperty(Guid propertyId)
        {
            var existingProperty = await this.dbContext.Properties.FindAsync(propertyId);

            if (existingProperty != null)
            {
                foreach (var offer in existingProperty.Offers.ToList())
                {
                    this.dbContext.Entry(offer).State = EntityState.Deleted;
                }

                this.dbContext.Properties.Remove(existingProperty);
                await this.dbContext.SaveChangesAsync();
            }

            return existingProperty;
        }

        private PropTradeDbContext dbContext;
    }
}
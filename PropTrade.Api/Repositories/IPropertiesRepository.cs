using PropTrade.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PropTrade.Api.Repositories
{
    public interface IPropertiesRepository
    {
        IQueryable<Property> GetPropertiesQuery();

        Task<Property> GetProperty(Guid propertyId);

        Task<IList<Property>> GetProperties(int? skip, int? take);

        Task<bool> CreateProperty(Property newProperty);

        Task<bool> UpdateProperty(Property modifiedProperty);

        Task<Property> DeleteProperty(Guid propertyId);
    }
}
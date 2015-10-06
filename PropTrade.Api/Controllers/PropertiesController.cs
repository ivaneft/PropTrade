using PropTrade.Api.Authentication;
using PropTrade.Api.Dtos;
using PropTrade.Api.Helpers;
using PropTrade.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PropTrade.Api.Controllers
{
    [CustomBasicAuthentication]
    [Authorize]
    public class PropertiesController : ApiController
    {
        public PropertiesController()
        {
            this.propertiesRepository = new PropertiesRepository();
        }

        public PropertiesController(IPropertiesRepository propertiesRepository)
        {
            this.propertiesRepository = propertiesRepository;
        }

        // GET: api/v1/Properties
        [ResponseType(typeof(IEnumerable<PropertyDto>))]
        public async Task<IHttpActionResult> GetProperties(int skip = 0, int take = 50)
        {
            var dbResult = await this.propertiesRepository.GetProperties(skip, take);
            IEnumerable<PropertyDto> properties = dbResult.Select(p => Mapper.MapPropertyEntityToDto(p, true));

            return Ok(properties);
        }

        // GET: api/v1/Properties?userId=5
        [HttpGet]
        [ResponseType(typeof(List<PropertyDto>))]
        public async Task<IHttpActionResult> GetPropertiesForUser(Guid userId)
        {
            var dbResult = await this.propertiesRepository
                .GetPropertiesQuery()
                .Where(p => p.OwnerId == userId)
                .ToListAsync();
            IEnumerable<PropertyDto> properties = dbResult.Select(p => Mapper.MapPropertyEntityToDto(p, true));

            return Ok(properties);
        }

        // GET: api/v1/Properties/5
        [ResponseType(typeof(PropertyDto))]
        public async Task<IHttpActionResult> GetProperty(Guid id)
        {
            var dbResult = await this.propertiesRepository.GetProperty(id);

            if (dbResult == null)
            {
                return NotFound();
            }

            PropertyDto property = Mapper.MapPropertyEntityToDto(dbResult, true);

            return Ok(property);
        }

        // PUT: api/v1/Properties/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProperty(Guid id, PropertyDto propertyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (propertyDto.Id == Guid.Empty)
            {
                propertyDto.Id = id;
            }

            await this.propertiesRepository.UpdateProperty(Mapper.MapPropertyDtoToEntity(propertyDto));

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/v1/Properties
        [ResponseType(typeof(PropertyDto))]
        public async Task<IHttpActionResult> PostProperty(PropertyDto propertyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            propertyDto.Id = Guid.NewGuid();
            propertyDto.Created = DateTime.UtcNow;
            await this.propertiesRepository.CreateProperty(Mapper.MapPropertyDtoToEntity(propertyDto));

            return CreatedAtRoute("DefaultApi", new { id = propertyDto.Id }, propertyDto);
        }

        // DELETE: api/v1/Properties/5
        [ResponseType(typeof(PropertyDto))]
        public async Task<IHttpActionResult> DeleteProperty(Guid id)
        {
            var deletedProperty = await this.propertiesRepository.DeleteProperty(id);

            if (deletedProperty == null)
            {
                return NotFound();
            }

            return Ok(Mapper.MapPropertyEntityToDto(deletedProperty, false));
        }        
        
        private IPropertiesRepository propertiesRepository;
    }
}
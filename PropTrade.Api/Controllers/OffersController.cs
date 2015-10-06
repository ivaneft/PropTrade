using PropTrade.Api.Authentication;
using PropTrade.Api.Dtos;
using PropTrade.Api.Helpers;
using PropTrade.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PropTrade.Api.Controllers
{
    [CustomBasicAuthentication]
    [Authorize]
    public class OffersController : ApiController
    {
        public OffersController()
        {
            this.offersRepository = new OffersRepository();
        }

        public OffersController(IOffersRepository offersRepository)
        {
            this.offersRepository = offersRepository;
        }

        // GET: api/v1/Offers
        [ResponseType(typeof(IEnumerable<OfferDto>))]
        public async Task<IHttpActionResult> GetOffers(int skip = 0, int take = 50)
        {
            var dbResult = await this.offersRepository.GetOffers(skip, take);
            IEnumerable<OfferDto> offers = dbResult.Select(o => Mapper.MapOfferEntityToDto(o, true));

            return Ok(offers);
        }

        // GET: api/v1/Offers/5
        [ResponseType(typeof(OfferDto))]
        public async Task<IHttpActionResult> GetOffer(Guid id)
        {
            var dbResult = await this.offersRepository.GetOffer(id);

            if(dbResult == null)
            {
                return NotFound();
            }

            OfferDto offer = Mapper.MapOfferEntityToDto(dbResult, true);

            return Ok(offer);
        }

        // PUT: api/v1/Offers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOffer(Guid id, OfferDto offerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (offerDto.Id == Guid.Empty)
            {
                offerDto.Id = id;
            }

            await this.offersRepository.UpdateOffer(Mapper.MapOfferDtoToEntity(offerDto));
            
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/v1/Offers
        [ResponseType(typeof(OfferDto))]
        public async Task<IHttpActionResult> PostOffer(OfferDto offerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            offerDto.Id = Guid.NewGuid();
            offerDto.Created = DateTime.UtcNow;
            await this.offersRepository.CreateOffer(offerDto.PropertyId, Mapper.MapOfferDtoToEntity(offerDto));

            return CreatedAtRoute("DefaultApi", new { id = offerDto.Id }, offerDto);
        }

        // DELETE: api/v1/Offers/5
        [ResponseType(typeof(OfferDto))]
        public async Task<IHttpActionResult> DeleteOffer(Guid id)
        {            
            var deletedOffer = await this.offersRepository.DeleteOffer(id);

            if (deletedOffer == null)
            {
                return NotFound();
            }

            return Ok(deletedOffer);
        }

        private IOffersRepository offersRepository;
    }
}
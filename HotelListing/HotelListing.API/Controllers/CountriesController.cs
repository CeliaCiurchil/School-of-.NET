using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using AutoMapper;
using HotelListing.API.Contracts;
using Microsoft.AspNetCore.Authorization;
using HotelListing.API.Exceptions;
using Asp.Versioning;
using HotelListing.API.Models;
using Microsoft.AspNetCore.OData.Query;

//dont talk with context here
namespace HotelListing.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0", Deprecated =true)]
    public class CountriesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICountriesRepository _countriesRepository;
        private readonly ILogger<CountriesController> _logger;
        public CountriesController(ICountriesRepository countriesRepository,IMapper mapper, ILogger<CountriesController> logger)
        {
            _countriesRepository = countriesRepository;
            this.mapper = mapper;
            _logger = logger;
        }

        // GET: api/Countries
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            var countries = await _countriesRepository.GetAllAsync();
            var records = mapper.Map<List<GetCountryDto>>(countries);
            return records;
        }

        //Get api/Countries/?StartIndex=0&PageSize=25&PageNumber=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetPagedCountries([FromQuery] QueryParameters queryParameters)
        {
            var pagedCountriesResults = await _countriesRepository.GetAllAsync<GetCountryDto>(queryParameters);
            return Ok(pagedCountriesResults);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await _countriesRepository.GetDetails(id);

            if (country == null)
            {
                throw new NotFoundException(nameof(country),id);
            }

            var countryDto = mapper.Map<CountryDto>(country);

            return countryDto;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
        {
            if (id != updateCountryDto.Id)
            {
                return BadRequest();
            }

            var country = await _countriesRepository.GetAsync(id);
            if(country == null)
            {
                return NotFound();
            }

            mapper.Map(updateCountryDto, country); //auto changed this to modify, is being trapped and ef KNOWS

            try
            {
                await _countriesRepository.UpdateAsync(country);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountry)
        {
            var country = mapper.Map<Country>(createCountry);

            await _countriesRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);// return the 201 status code
        }

        [Authorize(Roles ="Administrator")]
        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _countriesRepository.GetAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            await _countriesRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepository.Exists(id);
        }
    }
}

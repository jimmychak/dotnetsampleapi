using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sample.API.Entities;
using Sample.API.Models;
using Sample.API.Services;
using System;
using System.Collections.Generic;

namespace Sample.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _repo;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cityEntities = _repo.GetCities();
            return Ok(_mapper.Map<IEnumerable<CityDto>>(cityEntities));
        }

        [HttpGet("{id}", Name = "GetCityById")]
        public IActionResult GetCityById(int id)
        { 
            var city = _repo.GetCity(id);

            if (city == null)
                return NotFound();

            return Ok(_mapper.Map<CityDto>(city));
        }

        [HttpPost]
        public IActionResult CreateCity([FromBody] CityDto city)
        {
            var newCity = _mapper.Map<City>(city);

            _repo.AddCity(newCity);
            _repo.Save();

            var createdCityToReturn = _mapper.Map<Models.CityDto>(newCity);

            return CreatedAtRoute("GetCityById", new { id = createdCityToReturn.Id }, createdCityToReturn);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCity(int id, [FromBody] CityDto city)
        {
            var cityEntity = _repo.GetCity(id);
            if (cityEntity == null)
                return NotFound();

            _mapper.Map(city, cityEntity);
            _repo.UpdateCity(cityEntity);
            _repo.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCity(int id)
        {
            var city = _repo.GetCity(id);
            if (city == null)
                return NotFound();

            _repo.DeleteCity(city);

            _repo.Save();

            return NoContent();
        }
    }
}

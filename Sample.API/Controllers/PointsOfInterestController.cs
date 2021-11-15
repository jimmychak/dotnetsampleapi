using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Sample.API.Entities;
using Sample.API.Models;
using Sample.API.Services;
using System;
using System.Collections.Generic;

namespace Sample.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ICityInfoRepository _repo;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ICityInfoRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            if (!_repo.CityExists(cityId))
            {
                return NotFound();
            }

            var pointsOfInterestForCity = _repo.GetPointsOfInterestForCity(cityId);
            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
        }

        [HttpGet("{id}", Name = "GetPointOfInterestById")]
        public IActionResult GetPointOfInterestById(int cityId, int id)
        {
            if (!_repo.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = _repo.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterest == null)
                return NotFound();

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        [HttpPost]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestDto pointOfInterest)
        {
            if (!_repo.CityExists(cityId))
                return NotFound();

            var newPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterest);

            _repo.AddPointOfInterestForCity(cityId, newPointOfInterest);
            _repo.Save();

            var createdPointOfInterestToReturn = _mapper.Map<Models.PointOfInterestDto>(newPointOfInterest);

            return CreatedAtRoute("GetPointOfInterestById", new { cityId, id = createdPointOfInterestToReturn.Id }, createdPointOfInterestToReturn);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestDto pointOfInterestForUpdate)
        {
            if (!_repo.CityExists(cityId))
                return NotFound();

            var pointOfInterestEntity = _repo.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
                return NotFound();

            _mapper.Map(pointOfInterestForUpdate, pointOfInterestEntity);
            _repo.UpdatePointOfInterestForCity(cityId, pointOfInterestEntity);
            _repo.Save();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id, 
            [FromBody] JsonPatchDocument<PointOfInterestDto> patchDocument)
        {
            if (!_repo.CityExists(cityId))
                return NotFound();

            var pointOfInterestEntity = _repo.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
                return NotFound();

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestDto>(pointOfInterestEntity);
            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            _repo.UpdatePointOfInterestForCity(cityId, pointOfInterestEntity);
            _repo.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if (!_repo.CityExists(cityId))
                return NotFound();

            var pointOfInterestEntity = _repo.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
                return NotFound();

            _repo.DeletePointOfInterestForCity(pointOfInterestEntity);

            _repo.Save();

            return NoContent();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Sample.API.Contexts;
using Sample.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities.Include(c => c.PointsOfInterest).ToList();
        }

        public City GetCity(int cityId)
        {
            return _context.Cities.Include(c => c.PointsOfInterest).
                FirstOrDefault(c => c.Id == cityId);
        }

        public void AddCity(City city)
        {
            _context.Cities.Add(city);
        }

        public void UpdateCity(City city)
        {

        }

        public void DeleteCity(City city)
        {
            _context.Cities.Remove(city);
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return _context.PointsOfInterest
                .FirstOrDefault(p => p.CityId == cityId && p.Id == pointOfInterestId);
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId)
        {
            return _context.PointsOfInterest.Where(p => p.CityId == cityId).ToList();
        }

        public bool CityExists(int cityId)
        {
            return _context.Cities.Any(c => c.Id == cityId);
        }

        public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId);
            city.PointsOfInterest.Add(pointOfInterest);
        }

        public void UpdatePointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {

        }

        public void DeletePointOfInterestForCity(PointOfInterest pointOfInterest)
        {
            _context.Remove(pointOfInterest);
        }

        public bool Save()
        {
            return(_context.SaveChanges() >= 0);
        }
    }
}

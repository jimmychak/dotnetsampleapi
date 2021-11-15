using Sample.API.Entities;
using System.Collections.Generic;

namespace Sample.API.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();
        City GetCity(int cityId);
        void AddCity(City city);
        void UpdateCity(City city);
        void DeleteCity(City city);
        IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId);
        PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId);
        bool CityExists(int cityId);
        void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);
        public void UpdatePointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);
        public void DeletePointOfInterestForCity(PointOfInterest pointOfInterest);

        bool Save();
    }
}

using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using Sample.API.Controllers;
using Sample.API.Services;
using Sample.API.Entities;
using Sample.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Sample.UnitTest
{
    public class TestCitiesController
    {
        [Test]
        public void GetCities_ShouldReturnAll()
        {
            var cities = GetSampleCities();
            var repo = Substitute.For<ICityInfoRepository>();
            repo.GetCities().Returns(cities);

            var mapper = Substitute.For<IMapper>();
            var cityDtos = new List<CityDto>();
            foreach(var city in cities)
            {
                cityDtos.Add(new CityDto()
                {
                    Id = city.Id,
                    Name = city.Name,
                    Description = city.Description
                });
            };
            mapper.Map<IEnumerable<CityDto>>(cities).Returns(cityDtos);

            var controller = new CitiesController(repo, mapper);

            var actionResult = controller.GetCities();
            var objectResult = actionResult as OkObjectResult;
            var value = objectResult.Value as IList<CityDto>;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(value, Is.Not.Null);
            Assert.That(value.Count, Is.EqualTo(cities.Count));
        }

        [Test]
        public void GetCityById_ShouldReturnOK()
        {
            var city = GetSampleCity();
            var repo = Substitute.For<ICityInfoRepository>();
            repo.GetCity(1).Returns(city);

            var mapper = Substitute.For<IMapper>();
            var cityDto = new CityDto() 
            { 
                Id = city.Id, 
                Name = city.Name, 
                Description = city.Description
            };
            mapper.Map<CityDto>(city).Returns(cityDto);

            var controller = new CitiesController(repo, mapper);

            var actionResult = controller.GetCityById(1);
            var objectResult = actionResult as OkObjectResult;
            var value = objectResult.Value as CityDto;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(value, Is.Not.Null);
            Assert.That(value.Id, Is.EqualTo(city.Id));
            Assert.That(value.Name, Is.EqualTo(city.Name));
            Assert.That(value.Description, Is.EqualTo(city.Description));
        }

        [Test]
        public void GetCityByIdWithPointsOfInterest_ShouldReturnOK()
        {
            var city = GetSampleCity();
            var repo = Substitute.For<ICityInfoRepository>();
            repo.GetCity(1).Returns(city);

            var mapper = Substitute.For<IMapper>();
            var pointsOfInterestDto = new List<PointOfInterestDto>();
            foreach(var pointOfInterest in city.PointsOfInterest)
            {
                pointsOfInterestDto.Add(new PointOfInterestDto() 
                {
                    Id = pointOfInterest.Id,
                    Name = pointOfInterest.Name,
                    Description = pointOfInterest.Description
                });
            }

            var cityDto = new CityDto()
            {
                Id = city.Id,
                Name = city.Name,
                Description = city.Description,
                PointsOfInterest = pointsOfInterestDto
            };
            mapper.Map<CityDto>(city).Returns(cityDto);

            var controller = new CitiesController(repo, mapper);

            var actionResult = controller.GetCityById(1);
            var objectResult = actionResult as OkObjectResult;
            var value = objectResult.Value as CityDto;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(value, Is.Not.Null);
            Assert.That(value.Id, Is.EqualTo(city.Id));
            Assert.That(value.Name, Is.EqualTo(city.Name));
            Assert.That(value.Description, Is.EqualTo(city.Description));
            Assert.That(value.PointsOfInterest.Count, Is.EqualTo(city.PointsOfInterest.Count));
        }

        [Test]
        public void GetCityById_ShouldReturnNotFound()
        {
            var repo = Substitute.For<ICityInfoRepository>();
            var mapper = Substitute.For<IMapper>();

            var controller = new CitiesController(repo, mapper);

            var actionResult = controller.GetCityById(10);

            Assert.That(actionResult, Is.InstanceOf(typeof(NotFoundResult)));
        }

        List<City> GetSampleCities()
        {
            return new List<City>()
            {
                new City()
                {
                    Id = 1,
                    Name = "Leeds",
                    Description = "A city in the northern English county of Yorkshire"
                },
                new City()
                {
                    Id = 2,
                    Name = "Sheffield",
                    Description = "A city in the English county of South Yorkshire"
                }
            };
        }


        City GetSampleCity()
        {
            return new City()
            {
                Id = 1,
                Name = "Leeds",
                Description = "A city in the northern English county of Yorkshire",
                PointsOfInterest = new List<PointOfInterest>(){
                    new PointOfInterest()
                    {
                        Id = 1,
                        CityId = 1,
                        Name = "Kirkstall Abbey",
                        Description = "A ruined Cistercian monastery"
                    },
                    new PointOfInterest()
                    {
                        Id = 2,
                        CityId = 1,
                        Name = "Kirkgate Market",
                        Description = "One of the largest indoor markets in Europe"
                    }
                }
            };
        }
    }
}
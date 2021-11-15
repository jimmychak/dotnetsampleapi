using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Sample.API.Controllers;
using Sample.API.Entities;
using Sample.API.Models;
using Sample.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.UnitTest
{
    public class TestPointsOfInterestController
    {
        [Test]
        public void GetPointsOfInterest_ShouldReturnSameCount()
        {
            var pointsOfInterest = GetSamplePointsOfInterest();
            var repo = Substitute.For<ICityInfoRepository>();
            repo.GetPointsOfInterestForCity(1).Returns(pointsOfInterest);
            repo.CityExists(1).Returns(true);

            var mapper = Substitute.For<IMapper>();
            var pointOfInterestDtos = new List<PointOfInterestDto>();
            foreach (var pointOfInterest in pointsOfInterest)
            {
                pointOfInterestDtos.Add(new PointOfInterestDto()
                {
                    Id = pointOfInterest.Id,
                    Name = pointOfInterest.Name,
                    Description = pointOfInterest.Description
                });
            };
            mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterest).Returns(pointOfInterestDtos);

            var controller = new PointsOfInterestController(repo, mapper);

            var actionResult = controller.GetPointsOfInterest(1);
            var objectResult = actionResult as OkObjectResult;
            var value = objectResult.Value as IList<PointOfInterestDto>;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(value, Is.Not.Null);
            Assert.That(value.Count, Is.EqualTo(pointsOfInterest.Count));
        }

        [Test]
        public void GetPointsOfInterest_ShouldReturnNotFound()
        {
            var repo = Substitute.For<ICityInfoRepository>();
            repo.CityExists(10).Returns(false);

            var mapper = Substitute.For<IMapper>();

            var controller = new PointsOfInterestController(repo, mapper);

            var actionResult = controller.GetPointsOfInterest(10);

            Assert.That(actionResult, Is.InstanceOf(typeof(NotFoundResult)));
        }

        [Test]
        public void GetPointOfInterestById_ShouldReturnOK()
        {
            var pointOfInterest = GetSamplePointOfInterest();
            var repo = Substitute.For<ICityInfoRepository>();
            repo.GetPointOfInterestForCity(1, 1).Returns(pointOfInterest);
            repo.CityExists(1).Returns(true);

            var mapper = Substitute.For<IMapper>();
            var pointOfInterestDto = new PointOfInterestDto()
            {
                Id = pointOfInterest.Id,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };
            mapper.Map<PointOfInterestDto>(pointOfInterest).Returns(pointOfInterestDto);

            var controller = new PointsOfInterestController(repo, mapper);

            var actionResult = controller.GetPointOfInterestById(1, 1);
            var objectResult = actionResult as OkObjectResult;
            var value = objectResult.Value as PointOfInterestDto;

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(value, Is.Not.Null);
            Assert.That(value.Id, Is.EqualTo(pointOfInterest.Id));
            Assert.That(value.Name, Is.EqualTo(pointOfInterest.Name));
            Assert.That(value.Description, Is.EqualTo(pointOfInterest.Description));
        }

        [Test]
        public void GetPointOfInterestById_ShouldReturnNotFound()
        {
            var repo = Substitute.For<ICityInfoRepository>();
            repo.CityExists(10).Returns(false);

            var mapper = Substitute.For<IMapper>();

            var controller = new PointsOfInterestController(repo, mapper);

            var actionResult = controller.GetPointOfInterestById(10, 1);
            var actionResult2 = controller.GetPointOfInterestById(10, 10);

            Assert.That(actionResult, Is.InstanceOf(typeof(NotFoundResult)));
            Assert.That(actionResult2, Is.InstanceOf(typeof(NotFoundResult)));
        }

        [Test]
        public void CreatePointOfInterest_ShouldReturnSameObject()
        {
            var repo = Substitute.For<ICityInfoRepository>();
            repo.CityExists(1).Returns(true);

            var pointOfInterest = new PointOfInterest() { Id = 10, Name = "Test Name", Description = "Test Description" };
            var pointOfInterestDto = new PointOfInterestDto() { Id = 10, Name = "Test Name", Description = "Test Description" };

            var mapper = Substitute.For<IMapper>();
            mapper.Map<PointOfInterest>(pointOfInterestDto).Returns(pointOfInterest);
            mapper.Map<PointOfInterestDto>(pointOfInterest).Returns(pointOfInterestDto);

            var controller = new PointsOfInterestController(repo, mapper);

            var actionResult = controller.CreatePointOfInterest(1, pointOfInterestDto);
            var objectResult = actionResult as CreatedAtRouteResult;
            var value = objectResult.Value as PointOfInterestDto;

            Assert.That(objectResult, Is.Not.Null);
            Assert.AreEqual(objectResult.RouteName, "GetPointOfInterestById");
            Assert.AreEqual(objectResult.RouteValues["id"], value.Id);
            Assert.AreEqual(value.Name, pointOfInterestDto.Name);
        }

        [Test]
        public void CreatePointOfInterest_ShouldReturnNotFound()
        {
            var repo = Substitute.For<ICityInfoRepository>();
            repo.CityExists(10).Returns(false);

            var mapper = Substitute.For<IMapper>();

            var controller = new PointsOfInterestController(repo, mapper);

            var pointOfInterestDto = new PointOfInterestDto() { Name = "Test Name", Description = "Test Description" };
            var actionResult = controller.CreatePointOfInterest(10, pointOfInterestDto);
            Assert.That(actionResult, Is.InstanceOf(typeof(NotFoundResult)));
        }

        [Test]
        public void UpdatePointOfInterest_ShouldReturnNoContent()
        {
            var repo = Substitute.For<ICityInfoRepository>();
            repo.CityExists(1).Returns(true);
            var pointOfInterest = new PointOfInterest()
            {
                Id = 1,
                CityId = 1,
                Name = "Kirkstall Abbey",
                Description = "A ruined Cistercian monastery"
            };
            repo.GetPointOfInterestForCity(1, 1).Returns(pointOfInterest);

            var pointOfInterestDto = new PointOfInterestDto() { Name = "Test Name", Description = "Test Description" };
            var mapper = Substitute.For<IMapper>();

            var controller = new PointsOfInterestController(repo, mapper);

            var actionResult = controller.UpdatePointOfInterest(1, 1, pointOfInterestDto);
            Assert.That(actionResult, Is.InstanceOf(typeof(NoContentResult)));
        }

        [Test]
        public void UpdatePointOfInterest_ShouldReturnNotFound()
        {
            var repo = Substitute.For<ICityInfoRepository>();
            repo.CityExists(10).Returns(false);

            var mapper = Substitute.For<IMapper>();

            var controller = new PointsOfInterestController(repo, mapper);

            var pointOfInterestDto = new PointOfInterestDto() { Name = "Test Name", Description = "Test Description" };
            var actionResult = controller.UpdatePointOfInterest(10, 1, pointOfInterestDto);
            Assert.That(actionResult, Is.InstanceOf(typeof(NotFoundResult)));
        }

        [Test]
        public void UpdatePointOfInterestWithNotExistCity_ShouldReturnNotFound()
        {
            var repo = Substitute.For<ICityInfoRepository>();
            repo.CityExists(10).Returns(false);

            var mapper = Substitute.For<IMapper>();

            var controller = new PointsOfInterestController(repo, mapper);

            var pointOfInterestDto = new PointOfInterestDto() { Name = "Test Name", Description = "Test Description" };
            var actionResult = controller.UpdatePointOfInterest(10, 1, pointOfInterestDto);
            Assert.That(actionResult, Is.InstanceOf(typeof(NotFoundResult)));
        }

        [Test]
        public void UpdatePointOfInterestWithNotExistPointOfInterest_ShouldReturnNotFound()
        {
            var repo = Substitute.For<ICityInfoRepository>();
            repo.CityExists(1).Returns(true);
            repo.GetPointOfInterestForCity(1, 10);

            var mapper = Substitute.For<IMapper>();

            var controller = new PointsOfInterestController(repo, mapper);

            var pointOfInterestDto = new PointOfInterestDto() { Name = "Test Name", Description = "Test Description" };
            var actionResult = controller.UpdatePointOfInterest(1, 10, pointOfInterestDto);
            Assert.That(actionResult, Is.InstanceOf(typeof(NotFoundResult)));
        }

        [Test]
        public void DeletePointOfInterest_ShouldReturnNoContent()
        {
            var repo = Substitute.For<ICityInfoRepository>();
            repo.CityExists(1).Returns(true);
            var pointOfInterest = new PointOfInterest()
            {
                Id = 1,
                CityId = 1,
                Name = "Kirkstall Abbey",
                Description = "A ruined Cistercian monastery"
            };
            repo.GetPointOfInterestForCity(1, 1).Returns(pointOfInterest);


            var mapper = Substitute.For<IMapper>();

            var controller = new PointsOfInterestController(repo, mapper);

            var actionResult = controller.DeletePointOfInterest(1, 1);
            Assert.That(actionResult, Is.InstanceOf(typeof(NoContentResult)));
        }

        [Test]
        public void DeletePointOfInterestWithNotExistCity_ShouldReturnNotFound()
        {
            var repo = Substitute.For<ICityInfoRepository>();
            repo.CityExists(10).Returns(false);

            var mapper = Substitute.For<IMapper>();

            var controller = new PointsOfInterestController(repo, mapper);

            var actionResult = controller.DeletePointOfInterest(10, 1);
            Assert.That(actionResult, Is.InstanceOf(typeof(NotFoundResult)));
        }

        [Test]
        public void DeletePointOfInterestWithNotExistPointOfInterest_ShouldReturnNotFound()
        {
            var repo = Substitute.For<ICityInfoRepository>();
            repo.CityExists(1).Returns(true);
            repo.GetPointOfInterestForCity(1, 10);

            var mapper = Substitute.For<IMapper>();

            var controller = new PointsOfInterestController(repo, mapper);

            var actionResult = controller.DeletePointOfInterest(1, 10);
            Assert.That(actionResult, Is.InstanceOf(typeof(NotFoundResult)));
        }

        List<PointOfInterest> GetSamplePointsOfInterest()
        {
            return  new List<PointOfInterest>()
            {
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
                    CityId = 2,
                    Name = "Kirkgate Market",
                    Description = "One of the largest indoor markets in Europe"
                }
            };
        }

        PointOfInterest GetSamplePointOfInterest()
        {
            return new PointOfInterest()
            {
                Id = 1,
                CityId = 1,
                Name = "Kirkstall Abbey",
                Description = "A ruined Cistercian monastery"
            };
        }
    }
}

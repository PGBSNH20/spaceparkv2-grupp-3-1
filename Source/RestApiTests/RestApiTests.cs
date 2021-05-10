using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using RestAPI.Controllers;
using RestAPI.Data;
using RestAPI.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq.EntityFrameworkCore;
using RestAPI.ParkingLogic;
using RestAPI.Requests;
using Xunit;

namespace RestApiTests
{
    public class RestApiTests
    {
        private ISpacePort _spacePort = new SpacePort();
        private IParking _parking = new Parking();
        private IDbQueries _dbQueries = new DbQueries();
        private IReceipt _receipt = new Receipt();
        private ICalculate _calculate = new Calculate();

        [Theory]
        [InlineData("Luke Skywalker")]
        [InlineData("Darth Vader")]
        [InlineData("Yoda")]
        [InlineData("LUKE SKYWALKER")]
        [InlineData("yoda")]
        [InlineData("DaRtH vAdER")]
        public void Park_Valid_Starwars_Character_And_Ship_Expect_Parked(string name)
        {
            //Arrange
            DbContextOptions<SpaceParkDbContext> options = new DbContextOptionsBuilder<SpaceParkDbContext>().Options;
            var moqContext = new Mock<SpaceParkDbContext>(options);
            var userController = new UserController(moqContext.Object, _receipt, _calculate, _dbQueries);
            IList<SpacePort> spacePorts = new List<SpacePort>() { new SpacePort() { Id = 1, Name = "Hilux" } };
            IList<Parking> parkings = new List<Parking>();
            IList<Size> sizes = new List<Size>();
            spacePorts[0].Parkings = new List<Parking>();


            ParkRequest request = new ParkRequest();
            request.PersonName = name;
            request.ShipName = "X-Wing";

            Size size = new Size()
            {
                Id = 1,
                Type = ParkingSize.Small
            };


            Parking parking = new Parking()
            {
                Id = 1,
                Size = size,
                SizeId = 1,
                SpacePortId = 1,
                CharacterName = null,
                SpaceshipName = null,
                Arrival = null
            };
            parkings.Add(parking);
            spacePorts[0].Parkings = parkings;

            moqContext.Setup(x => x.Sizes).ReturnsDbSet(sizes);
            moqContext.Setup(x => x.Parkings).ReturnsDbSet(parkings);
            moqContext.Setup(sp => sp.SpacePorts).ReturnsDbSet(spacePorts);


            //Act
            userController.Park(spacePorts[0].Id, request);
            var foundParking = spacePorts[0].Parkings.FirstOrDefault(p => p.CharacterName == request.PersonName);

            //Assert
            Assert.Equal(foundParking.CharacterName, request.PersonName);
        }

        [Fact]
        public void Park_Valid_Starwars_Character_And_Ship_Space_Port_Full_Expect_BadRequest()
        {
            //Arrange
            DbContextOptions<SpaceParkDbContext> options = new DbContextOptionsBuilder<SpaceParkDbContext>().Options;
            var moqContext = new Mock<SpaceParkDbContext>(options);
            var userController = new UserController(moqContext.Object, _receipt, _calculate, _dbQueries);
            IList<SpacePort> spacePorts = new List<SpacePort>() { new SpacePort() { Id = 1, Name = "Hilux" } };
            IList<Parking> parkings = new List<Parking>();
            IList<Size> sizes = new List<Size>();
            spacePorts[0].Parkings = new List<Parking>();


            ParkRequest request = new ParkRequest();
            request.PersonName = "Han Solo";
            request.ShipName = "X-Wing";

            Size size = new Size()
            {
                Id = 1,
                Type = ParkingSize.Small
            };

            Parking parking = new Parking()
            {
                Id = 1,
                Size = size,
                SizeId = 1,
                SpacePortId = 1,
                CharacterName = "Darth Vader",
                SpaceshipName = "Y-Wing",
                Arrival = DateTime.Now
            };
            parkings.Add(parking);
            spacePorts[0].Parkings = parkings;

            moqContext.Setup(x => x.Sizes).ReturnsDbSet(sizes);
            moqContext.Setup(x => x.Parkings).ReturnsDbSet(parkings);
            moqContext.Setup(sp => sp.SpacePorts).ReturnsDbSet(spacePorts);


            //Act
            var response = userController.Park(spacePorts[0].Id, request);

            //Assert
            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Theory]
        [InlineData("Luke Skywalker", "Toyota")]
        [InlineData("Darth Vader", "Something-Wing")]
        [InlineData("Yoda", "X-Wings")]
        public void Park_Valid_Starwars_Character_And_Invalid_Ship_Expect_Not_Parked(string name, string shipName)
        {
            //Arrange
            DbContextOptions<SpaceParkDbContext> options = new DbContextOptionsBuilder<SpaceParkDbContext>().Options;
            var moqContext = new Mock<SpaceParkDbContext>(options);
            var userController = new UserController(moqContext.Object, _receipt, _calculate, _dbQueries);
            IList<SpacePort> spacePorts = new List<SpacePort>() { new SpacePort() { Id = 1, Name = "Hilux" } };
            IList<Parking> parkings = new List<Parking>();
            IList<Size> sizes = new List<Size>();
            spacePorts[0].Parkings = new List<Parking>();


            ParkRequest request = new ParkRequest();
            request.PersonName = name;
            request.ShipName = shipName;

            Size size = new Size()
            {
                Id = 1,
                Type = ParkingSize.Small
            };

            Parking parking = new Parking()
            {
                Id = 1,
                Size = size,
                SizeId = 1,
                SpacePortId = 1,
                CharacterName = null,
                SpaceshipName = null,
                Arrival = null
            };
            parkings.Add(parking);

            spacePorts[0].Parkings = parkings;

            moqContext.Setup(x => x.Sizes).ReturnsDbSet(sizes);
            moqContext.Setup(x => x.Parkings).ReturnsDbSet(parkings);
            moqContext.Setup(sp => sp.SpacePorts).ReturnsDbSet(spacePorts);

            //Act
            userController.Park(spacePorts[0].Id, request);
            var foundParking = spacePorts[0].Parkings.FirstOrDefault(p => p.CharacterName == request.PersonName);

            //Assert
            Assert.Null(foundParking);
        }

        [Theory]
        [InlineData("Brad Pitt")]
        [InlineData("Spuute")]
        [InlineData("Luke Skyscraper")]
        [InlineData("Darth vaden")]
        public void Park_Invalid_Starwars_Character_And_Valid_Ship_Expect_Not_Parked(string name)
        {
            //Arrange
            DbContextOptions<SpaceParkDbContext> options = new DbContextOptionsBuilder<SpaceParkDbContext>().Options;
            var moqContext = new Mock<SpaceParkDbContext>(options);
            var userController = new UserController(moqContext.Object, _receipt, _calculate, _dbQueries);
            IList<SpacePort> spacePorts = new List<SpacePort>() { new SpacePort() { Id = 1, Name = "Hilux" } };
            IList<Parking> parkings = new List<Parking>();
            IList<Size> sizes = new List<Size>();
            spacePorts[0].Parkings = new List<Parking>();


            ParkRequest request = new ParkRequest();
            request.PersonName = name;
            request.ShipName = "X-Wing";

            Size size = new Size()
            {
                Id = 1,
                Type = ParkingSize.Small
            };


            Parking parking = new Parking()
            {
                Id = 1,
                Size = size,
                SizeId = 1,
                SpacePortId = 1,
                CharacterName = null,
                SpaceshipName = null,
                Arrival = null
            };
            parkings.Add(parking);
            spacePorts[0].Parkings = parkings;

            moqContext.Setup(x => x.Sizes).ReturnsDbSet(sizes);
            moqContext.Setup(x => x.Parkings).ReturnsDbSet(parkings);
            moqContext.Setup(sp => sp.SpacePorts).ReturnsDbSet(spacePorts);


            //Act
            userController.Park(spacePorts[0].Id, request);
            var foundParking = spacePorts[0].Parkings.FirstOrDefault(p => p.CharacterName == request.PersonName);

            //Assert
            Assert.Null(foundParking);
        }

        [Theory]
        [InlineData("Luke Skywalker", 5)]
        [InlineData("Darth vader", 8)]
        [InlineData("Yoda", 2)]
        public void Park_Valid_Starwars_Character_And_Ship_Then_Get_Active_Parkings_Expect_Found(string name, int expected)
        {
            //Arrange
            DbContextOptions<SpaceParkDbContext> options = new DbContextOptionsBuilder<SpaceParkDbContext>().Options;
            var moqContext = new Mock<SpaceParkDbContext>(options);
            var userController = new UserController(moqContext.Object, _receipt, _calculate, _dbQueries);
            IList<SpacePort> spacePorts = new List<SpacePort>() { new SpacePort() { Id = 1, Name = "Hilux" } };
            IList<Parking> parkings = new List<Parking>();
            IList<Size> sizes = new List<Size>();
            spacePorts[0].Parkings = new List<Parking>();

            ParkRequest request = new ParkRequest();
            request.PersonName = name;
            request.ShipName = "X-Wing";

            Size size = new Size()
            {
                Id = 1,
                Type = ParkingSize.Small
            };

            for (int i = 1; i < expected + 1; i++)
            {
                Parking parking = new Parking()
                {
                    Id = i,
                    Size = size,
                    SizeId = 1,
                    SpacePortId = 1,
                    CharacterName = name,
                    SpaceshipName = "X-Wing",
                    Arrival = DateTime.Now
                };
                parkings.Add(parking);
            }

            spacePorts[0].Parkings = parkings;

            moqContext.Setup(x => x.Sizes).ReturnsDbSet(sizes);
            moqContext.Setup(x => x.Parkings).ReturnsDbSet(parkings);
            moqContext.Setup(sp => sp.SpacePorts).ReturnsDbSet(spacePorts);


            //Act
            var okObjectResult = userController.ActiveParkings(request.PersonName) as OkObjectResult;
            var activeParkings = okObjectResult.Value as List<Parking>;

            //Assert
            Assert.Equal(activeParkings.Count, expected);
        }

        [Theory]
        [InlineData("Luke Skywalker", "X-Wing", 1, true)]
        [InlineData("Luke Skywalker", "X-Wing", 4, false)]
        [InlineData("Darth Vader", "Y-Wing", 4, false)]
        public void Unpark_With_Correct_Parking_Id_And_Character_And_Ship_Name_Check_Null_In_Parking_Name_Property_Expect_True_Or_False(string name, string shipName, int parkingId, bool expected)
        {
            //Arrange
            DbContextOptions<SpaceParkDbContext> options = new DbContextOptionsBuilder<SpaceParkDbContext>().Options;
            var moqContext = new Mock<SpaceParkDbContext>(options);
            var userController = new UserController(moqContext.Object, _receipt, _calculate, _dbQueries);
            IList<SpacePort> spacePorts = new List<SpacePort>() { new SpacePort() { Id = 1, Name = "Hilux" } };
            IList<Parking> parkings = new List<Parking>();
            IList<Size> sizes = new List<Size>();
            IList<Receipt> receipts = new List<Receipt>();
            spacePorts[0].Parkings = new List<Parking>();


            ParkRequest request = new ParkRequest();
            request.PersonName = name;
            request.ShipName = shipName;

            Size size = new Size()
            {
                Id = 1,
                Type = ParkingSize.Small
            };
            sizes.Add(size);

            Parking parking = new Parking()
            {
                Id = 1,
                Size = size,
                SizeId = 1,
                SpacePortId = 1,
                CharacterName = name,
                SpaceshipName = shipName,
                Arrival = DateTime.Now.AddHours(-2)
            };
            parkings.Add(parking);
            spacePorts[0].Parkings = parkings;

            moqContext.Setup(x => x.Sizes).ReturnsDbSet(sizes);
            moqContext.Setup(x => x.Parkings).ReturnsDbSet(parkings);
            moqContext.Setup(sp => sp.Receipts).ReturnsDbSet(receipts);
            moqContext.Setup(sp => sp.SpacePorts).ReturnsDbSet(spacePorts);

            //Act
            userController.Unpark(parkingId, request);
            var onlyParking = moqContext.Object.Parkings.FirstOrDefault();

            //Assert
            Assert.Equal(onlyParking.CharacterName == null, expected);
        }

        [Theory]
        [InlineData(0, ParkingSize.Small, 100)]
        [InlineData(0, ParkingSize.Medium, 400)]
        [InlineData(0, ParkingSize.Large, 900)]
        [InlineData(0, ParkingSize.VeryLarge, 6000)]
        [InlineData(1, ParkingSize.Small, 300)]
        [InlineData(1, ParkingSize.Medium, 1200)]
        [InlineData(1, ParkingSize.Large, 2700)]
        [InlineData(1, ParkingSize.VeryLarge, 18000)]
        public void Calculate_Price_When_X_Amount_Of_Minutes_Passed_And_Parking_Size_Equals_Y(double minutes, ParkingSize size, double expectedPrice)
        {
            //Arrange
            Calculate calculate = new Calculate();

            //Act
            var price = calculate.Price(minutes, size);

            //Assert
            Assert.Equal(expectedPrice, price);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        public void Get_All_Space_Ports_Expect_X_Amount_Of_Space_Ports(int numberOfPorts)
        {
            //Arrange
            DbContextOptions<SpaceParkDbContext> options = new DbContextOptionsBuilder<SpaceParkDbContext>().Options;
            var moqContext = new Mock<SpaceParkDbContext>(options);
            var adminController = new AdminController(moqContext.Object, _spacePort, _parking);
            // SpaceParkDbContext dbContext, ISpacePort spacePort, IParking parking
            IList<SpacePort> spacePorts = new List<SpacePort>();

            for (int i = 1; i < numberOfPorts + 1; i++)
            {
                spacePorts.Add(new SpacePort()
                {
                    Id = i,
                    Name = "Newport"
                });
            }

            moqContext.Setup(sp => sp.SpacePorts).ReturnsDbSet(spacePorts);

            //Act
            var response = adminController.GetAllSpacePorts() as OkObjectResult;
            var spacePortList = response.Value as List<SpacePort>;
            //Assert
            Assert.Equal(numberOfPorts, spacePortList.Count);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RestAPI.Data;
using RestAPI.Models;
using RestAPI.ParkingLogic;
using RestAPI.Requests;
using RestAPI.Swapi;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private SpaceParkDbContext _dbContext;
        private IReceipt _receipt;
        private ICalculate _calculate;
        private IDbQueries _dbQueries;

        public UserController(SpaceParkDbContext dbContext, IReceipt receipt, ICalculate calculate, IDbQueries dbQueries)
        {
            _dbContext = dbContext;
            _receipt = receipt;
            _calculate = calculate;
            _dbQueries = dbQueries;
        }

        // GET: api/<SpaceParkController>
        [HttpGet("[action]")]
        public IActionResult ParkingHistory([FromBody] string name)
        {
            var receipts = _dbContext.Receipts.Where(r => r.Name.ToLower() == name.ToLower()).ToList();

            return Ok(receipts);
        }

        // GET api/<SpaceParkController>/5
        [HttpGet("[action]")]
        public IActionResult ActiveParkings([FromBody] string name)
        {
            var activeParkings = _dbContext.Parkings.Where(p => p.CharacterName.ToLower() == name.ToLower()).ToList();

            return Ok(activeParkings);
        }

        // PUT api/User/Park/AtSpacePort/5
        [HttpPut("[action]/AtSpacePort/{id}")]
        public IActionResult Park(int id, [FromBody] ParkRequest request)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            var validSpacePort = _dbContext.SpacePorts.FirstOrDefault(sp => sp.Id == id);
            if (validSpacePort == null)
            {
                return BadRequest($"There is no existing spaceport with id:{id}.");
            }

            var unoccupiedParkings = _dbQueries.FindUnoccupiedParkings(_dbContext, id);

            if (unoccupiedParkings.Count > 0)
            {
                var validPerson = Validate.Person(request.PersonName);
                var validShip = Validate.Starship(request.ShipName);
                double length = 0;

                if (validPerson.Result == false || validShip.Result == null)
                {
                    return Unauthorized("Invalid character or ship.");
                }

                length = double.Parse(validShip.Result.Length);

                var parkingId = _dbQueries.CorrectSizeParking(length, id, _dbContext);

                if (parkingId != 0)
                {
                    bool parked = _dbQueries.ParkVehicle(parkingId, _dbContext, request.PersonName, request.ShipName);

                    if (parked)
                    {
                        return Ok("Vehicle parked.");
                    }
                    return BadRequest("Failed to park vehicle");
                }

                return BadRequest("No suitable parkings for your ship length.");
            }
            return BadRequest("Space port is full.");
        }

        // PUT api/User/Unpark/5
        [HttpPut("[action]/{id}")]
        public IActionResult Unpark(int id, [FromBody] ParkRequest request)
        {
            IParking foundParking = _dbContext.Parkings.Include(p => p.Size).FirstOrDefault(p => p.Id == id && request.PersonName.ToLower() == p.CharacterName.ToLower() && request.ShipName.ToLower() == p.SpaceshipName.ToLower());
            
            if (foundParking != null)
            {
                _dbQueries.CreateReceipt(_receipt, foundParking, _calculate, _dbContext);

                return StatusCode(StatusCodes.Status200OK, $"Vehicle unparked.");
            }
            return BadRequest("Incorrect name, ship or parking spot id");
        }
    }
}


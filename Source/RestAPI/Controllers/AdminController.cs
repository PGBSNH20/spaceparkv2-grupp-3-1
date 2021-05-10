using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RestAPI.Data;
using RestAPI.Models;
using Microsoft.EntityFrameworkCore;
using RestAPI.Requests;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private SpaceParkDbContext _dbContext;
        private ISpacePort _spacePort { get; set; }
        private IParking _parking { get; set; }

        public AdminController(SpaceParkDbContext dbContext, ISpacePort spacePort, IParking parking)
        {
            _dbContext = dbContext;
            _spacePort = spacePort;
            _parking = parking;
        }

        // POST api/Admin/AddParking
        [HttpPost("[action]")]
        public IActionResult AddParking([FromBody] AddParkingRequest request)
        {
            var parking = new Parking()
            {
                SizeId = request.SizeId,
                SpacePortId = request.SpacePortId,
                CharacterName = null,
                SpaceshipName = null,
                Arrival = null
            };

            try
            {
                _dbContext.Parkings.Add(parking);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, "Parking Added to Database.");
            }
            catch
            {
                return StatusCode(StatusCodes.Status409Conflict, "An error occurred while adding new parking spot.");
            }
        }

        // DELETE api/Admin/RemoveParking/5
        [HttpDelete("[action]/{id}")]
        public IActionResult RemoveParking(int id)
        {
            var parking = _dbContext.Parkings.FirstOrDefault(p => p.Id == id);
            if (parking != null)
            {
                _dbContext.Parkings.Remove(parking);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, $"Parking with id:{parking.Id} was removed successfully.");
            }
            else
            {
                return BadRequest($"Parking with id:{id} was not found.");
            }
        }

        // GET: api/ManageSpacePorts/GetAllSpacePorts
        [HttpGet("[action]")]
        public IActionResult GetAllSpacePorts()
        {
            var spacePorts = _dbContext.SpacePorts.Include(sp => sp.Parkings).ThenInclude(p => p.Size).ToList();

            return Ok(spacePorts);
        }

        // POST api/ManageSpacePorts/AddSpacePort
        [HttpPost("[action]")]
        public IActionResult AddSpacePort([FromBody] string name)
        {
            try
            {
                _spacePort.Name = name;
                _spacePort.Parkings = new List<Parking>();
                for (int i = 0; i < 5; i++)
                {
                    _parking = new Parking();
                    _parking.SizeId = _dbContext.Sizes.Where(s => s.Type == ParkingSize.Small).Select(s => s.Id).FirstOrDefault();
                    _spacePort.Parkings.Add((Parking)_parking);
                }
                for (int i = 0; i < 3; i++)
                {
                    _parking = new Parking();
                    _parking.SizeId = _dbContext.Sizes.Where(s => s.Type == ParkingSize.Medium).Select(s => s.Id).FirstOrDefault();
                    _spacePort.Parkings.Add((Parking)_parking);
                }
                for (int i = 0; i < 2; i++)
                {
                    _parking = new Parking();
                    _parking.SizeId = _dbContext.Sizes.Where(s => s.Type == ParkingSize.Large).Select(s => s.Id).FirstOrDefault();
                    _spacePort.Parkings.Add((Parking)_parking);
                }

                _parking = new Parking();
                _parking.SizeId = _dbContext.Sizes.Where(s => s.Type == ParkingSize.VeryLarge).Select(s => s.Id).FirstOrDefault();
                _spacePort.Parkings.Add((Parking)_parking);
                _dbContext.SpacePorts.Add((SpacePort)_spacePort);

                _dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status201Created, $"SpacePort added id: {_spacePort.Id} name: {_spacePort.Name}");
            }
            catch
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "An error has occurred while adding a new spaceport.");
            }
        }

        // DELETE api/ManageSpacePorts/DeleteSpacePort/5
        [HttpDelete("[action]/{id}")]
        public IActionResult DeleteSpacePort(int id)
        {
            //TODO: Interface för spaceport
            SpacePort spacePort = _dbContext.SpacePorts.FirstOrDefault(s => s.Id == id);
            if (spacePort != null)
            {
                _dbContext.SpacePorts.Remove(spacePort);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status202Accepted, $"Space port deleted.");
            }

            return BadRequest("Space port was not found.");
        }
    }
}

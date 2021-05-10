using RestAPI.Models;
using RestAPI.ParkingLogic;
using System;
using System.Collections.Generic;
using RestAPI.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RestAPI.Controllers
{
    public class DbQueries : IDbQueries
    {
        public void CreateReceipt(IReceipt receipt, IParking foundParking, ICalculate calculate, SpaceParkDbContext dbContext)
        {
            receipt.Name = foundParking.CharacterName;
            receipt.Arrival = (DateTime)foundParking.Arrival;
            receipt.StarshipName = foundParking.SpaceshipName;
            receipt.Departure = DateTime.Now;
            receipt.Size = foundParking.Size;

            double diff = (receipt.Departure - receipt.Arrival).TotalMinutes;
            var price = calculate.Price(diff, foundParking.Size.Type);

            receipt.TotalAmount = price;

            dbContext.Receipts.Add((Receipt)receipt);

            foundParking.Arrival = null;
            foundParking.CharacterName = null;
            foundParking.SpaceshipName = null;
            dbContext.SaveChanges();
        }

        public ICollection<Parking> FindUnoccupiedParkings(SpaceParkDbContext dbContext, int id)
        {
            var unoccupiedParkings = (from sp in dbContext.SpacePorts
                join p in dbContext.Parkings
                    on sp.Id equals p.SpacePortId
                where p.SpacePortId == id && p.CharacterName == null
                select new Parking()
                {
                    Id = p.Id,
                    SizeId = p.SizeId,
                    CharacterName = p.CharacterName,
                    SpaceshipName = p.SpaceshipName,
                    Arrival = p.Arrival,
                    SpacePortId = p.SpacePortId
                }).ToList();

            return unoccupiedParkings;
        }

        public int CorrectSizeParking(double shipLength, int spacePortId, SpaceParkDbContext dbContext)
        {
            var size = shipLength switch
            {
                < 500 => ParkingSize.Small,
                < 1000 => ParkingSize.Medium,
                < 10000 => ParkingSize.Large,
                _ => ParkingSize.VeryLarge
            };

            //var parkingList = dbContext.Parkings.FirstOrDefault(p => p.Size.Type == size && string.IsNullOrEmpty(p.CharacterName));
            var parking = dbContext.Parkings.Include("Size").FirstOrDefault(p => p.Size.Type == size && p.CharacterName == null && p.SpacePortId == spacePortId);


            if (parking != null)
            {
                return parking.Id;
            }

            return 0;
        }

        public bool ParkVehicle(int parkingId, SpaceParkDbContext dbContext, string name, string shipName)
        {
            var foundParking = dbContext.Parkings.FirstOrDefault(p => p.Id == parkingId);
            try
            {
                foundParking.Arrival = DateTime.Now;
                foundParking.CharacterName = name;
                foundParking.SpaceshipName = shipName;
                dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
using System.Collections.Generic;
using RestAPI.Data;
using RestAPI.Models;
using RestAPI.ParkingLogic;

namespace RestAPI.Controllers
{
    public interface IDbQueries
    {
        void CreateReceipt(IReceipt receipt, IParking foundParking, ICalculate calculate, SpaceParkDbContext dbContext);
        ICollection<Parking> FindUnoccupiedParkings(SpaceParkDbContext dbContext, int id);
        int CorrectSizeParking(double shipLength, int spacePortId, SpaceParkDbContext dbContext);
        bool ParkVehicle(int parkingId, SpaceParkDbContext dbContext, string name, string shipName);
    }
}
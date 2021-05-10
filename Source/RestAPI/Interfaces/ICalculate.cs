using RestAPI.Models;

namespace RestAPI.ParkingLogic
{
    public interface ICalculate
    {
        double Price(double time, ParkingSize size);
    }
}
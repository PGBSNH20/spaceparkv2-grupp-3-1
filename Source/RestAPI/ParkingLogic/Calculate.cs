using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestAPI.Models;

namespace RestAPI.ParkingLogic
{
    public class Calculate : ICalculate
    {
        public double Price(double time, ParkingSize size)
        {
            var price = size switch
            {
                ParkingSize.Small => (Math.Round(time, 0) * 200) + 100,
                ParkingSize.Medium => (Math.Round(time, 0) * 800) + 400,
                ParkingSize.Large => (Math.Round(time, 0) * 1800) + 900,
                _ => (Math.Round(time, 0) * 12000) + 6000
            };
            return price;
        }
    }
}

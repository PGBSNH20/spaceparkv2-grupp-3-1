using System;

namespace RestAPI.Models
{
    public interface IReceipt
    {
        int Id { get; set; }
        Size Size { get; set; }
        int SizeId { get; set; }
        string Name { get; set; }
        string StarshipName { get; set; }
        DateTime Arrival { get; set; }
        DateTime Departure { get; set; }
        double TotalAmount { get; set; }
    }
}
using System;

namespace RestAPI.Models
{
    public interface IParking
    {
        int Id { get; set; }
        Size Size { get; set; }
        int SizeId { get; set; }
        int SpacePortId { get; set; }
        string CharacterName { get; set; }
        string SpaceshipName { get; set; }
        DateTime? Arrival { get; set; }
    }
}
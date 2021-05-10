using System.Collections.Generic;

namespace RestAPI.Models
{
    public interface ISpacePort
    {
        int Id { get; set; }
        string Name { get; set; }
        ICollection<Parking> Parkings { get; set; }
    }
}
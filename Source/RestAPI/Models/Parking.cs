using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Models
{
    public class Parking : IParking
    {
        public int Id { get; set; }
        public Size Size { get; set; }
        [Required]
        public int SizeId { get; set; }
        [Required]
        public int SpacePortId { get; set; }
        public string CharacterName { get; set; }
        public string SpaceshipName { get; set; }
        public DateTime? Arrival { get; set; }
    }

}

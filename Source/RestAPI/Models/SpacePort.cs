using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Models
{
    public class SpacePort : ISpacePort
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Parking> Parkings { get; set; }
    }
}

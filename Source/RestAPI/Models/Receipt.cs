using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace RestAPI.Models
{
    public class Receipt : IReceipt
    {
        public int Id { get; set; }
        public Size Size { get; set; }
        [Required]
        public int SizeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string StarshipName { get; set; }
        [Required]
        public DateTime Arrival { get; set; }
        [Required]
        public DateTime Departure { get; set; }
        [Required]
        public double TotalAmount { get; set; }
    }
}

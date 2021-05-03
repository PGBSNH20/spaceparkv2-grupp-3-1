﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Models
{
    public class SpacePort
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Parking> Parkings { get; set; }
    }
}
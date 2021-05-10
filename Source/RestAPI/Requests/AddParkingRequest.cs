using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Requests
{
    public class AddParkingRequest
    {
        public int SizeId { get; set; }
        public int SpacePortId { get; set; }
    }
}

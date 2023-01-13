using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    //A class with details about Character origin, built using Place
    public class PersonOrigin
    {
        public string name { get; set; }
        public string type { get; set; }
        public string dimension { get; set; }

        public PersonOrigin(Place place)
        {
            this.name = place.name;
            this.type = place.type;
            this.dimension = place.dimension;
        }
    }
}

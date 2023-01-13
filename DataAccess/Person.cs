using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    //Class that is built using Character and PersonOrigin that is used in GET method
    public class Person
    {
        public string name { get; set; }
        public string status { get; set; }
        public string species { get; set; }
        public string type { get; set; }
        public string gender { get; set; }
        public PersonOrigin origin { get; set; }
        
        public Person(Character character, PersonOrigin personOrigin)
        {
          this.name = character.name;
          this.status = character.status;
          this.species = character.species;
          this.type = character.type;
          this.gender = character.gender;
          this.origin = personOrigin;
        }
    }
}

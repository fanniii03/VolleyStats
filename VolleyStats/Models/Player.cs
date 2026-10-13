using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolleyStats.Models
{
    public  class Player
    {
        public string Name { get; set; } = ""; // alapérték hogy a szöveges tulajdonságok ne null értékkel induljanak
        public int JerseyNumber { get; set; }
        public string Position { get; set; } = "";
    }
}

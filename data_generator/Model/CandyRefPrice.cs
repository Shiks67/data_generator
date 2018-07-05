using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_generator.Model
{
    class CandyRefPrice
    {
        public int Candy_ref_id { get; set; }
        public int Candy_id { get; set; }
        public int Package_id { get; set; }
        public string Price { get; set; }
    }
}

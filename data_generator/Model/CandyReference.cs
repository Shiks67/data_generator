using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_generator.Model
{
    class CandyReference
    {
        public int Candy_ref_id { get; set; }
        public int Candy_id { get; set; }
        public int Color_id { get; set; }
        public int Variant_id { get; set; }
        public int Texture_id { get; set; }
        public int Packaging_id { get; set; }
        public int Candy_quantity { get; set; }
        public int Candy_ref_code { get; set; }
    }
}

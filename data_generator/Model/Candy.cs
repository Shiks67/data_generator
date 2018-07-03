using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_generator.Model
{
    class Candy
    {
        public int id { get; set; }
        public string name { get; set; }
        public int manufacturing_cost { get; set; }
        public int packaging_cost { get; set; }
        public int sending_cost { get; set; }
        public int general_cost { get; set; }
        public string bag_price { get; set; }
        public string box_price { get; set; }
        public string sample_price { get; set; }
        public string additive { get; set; }
        public string coating{ get; set; }
        public string aroma { get; set; }
        public string gelling { get; set; }
        public string sugar { get; set; }
    }
}

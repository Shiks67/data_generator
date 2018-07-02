using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_generator.Model
{
    class Order
    {
        public int order_id { get; set; }
        public int customer_id { get; set; }
        public int country_id { get; set; }
        public int total_price { get; set; }
        public string date { get; set; }
    }
}

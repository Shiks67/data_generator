using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_generator.Model
{
    class OrderDetails
    {
        public int order_details_id { get; set; }
        public int order_id { get; set; }
        public int candy_id { get; set; }
        public int quantity { get; set; }
    }
}

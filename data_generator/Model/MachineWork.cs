using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_generator.Model
{
    class MachineWork
    {
        public int Id { get; set; }
        public int Machine_id { get; set; }
        public int Candy_ref_id { get; set; }
        public string Date { get; set; }
        public int Quantity { get; set; }
    }
}

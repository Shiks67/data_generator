using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_generator.Model
{
    class MachineManufacture
    {
        public int Machine_id { get; set; }
        public int candy_variant_id { get; set; }
        public string Cadence { get; set; }
        public string Tool_change { get; set; }
    }
}

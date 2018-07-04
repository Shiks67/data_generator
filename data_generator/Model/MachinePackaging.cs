using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data_generator.Model
{
    class MachinePackaging
    {
        public int Machine_id { get; set; }
        public int Cadence { get; set; }
        public int Tool_change { get; set; }
        public int Packaging_id { get; set; }
    }
}

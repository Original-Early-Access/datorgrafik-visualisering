using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.KNNAlgorithm
{
    public class DataRow
    {
        public List<double> Values { get; set; } = new List<double>();
        public int LabelID { get; set; }
    }
}

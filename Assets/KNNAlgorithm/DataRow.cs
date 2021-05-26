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
        public DataPoint DataPoint;
        public int LabelID { get; set; }
        public string Label { get; set; }
    }
}

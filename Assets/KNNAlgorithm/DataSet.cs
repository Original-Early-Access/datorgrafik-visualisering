using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.KNNAlgorithm
{
    public class DataSet
    {
        public List<DataRow> DataRows { get; set; } = new List<DataRow>();
        public List<string> Labels { get; set; } = new List<string>();
    }
}

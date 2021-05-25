using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.KNNAlgorithm
{
    public class DataPoint
    {
        public int LabelID;
        public string Label;
        public float X;
        public float Y;
        public float Z;
        public List<float> features;
    }
}

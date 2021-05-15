using KnnConsoleAppForUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.KNNAlgorithm
{
    public class KNNController
    {

        private static readonly Lazy<KNNController> lazy = new Lazy<KNNController>(() => new KNNController());
        public static KNNController Instance { get { return lazy.Value; } }

        public void StartPrediction()
        {
            kNN kNN = new kNN();

            double[][] traindata = kNN.LoadData();
            int numfeatures = 3;
            int numclasses = 3;
            List<DataPoint> dataPoints = new List<DataPoint>
            {
                new DataPoint{X = 4.42f, Y = 1.75f, Z = 4.0f},
                new DataPoint{X = 2.0f, Y = 1.24f, Z = 2.5f},
                new DataPoint{X = 1.0f, Y = 0.75f, Z = 3.0f},
            };
            string[] labels = new string[3]
            {
                "Sol",
                "deprimerad",
                "månen"
            };
            int k = 1;
            foreach(var datapoint in dataPoints)
            {
                int predict = kNN.Classify(new double[3] { datapoint.X, datapoint.Y, datapoint.Z }, traindata, numclasses, k, numfeatures);
                datapoint.Label = labels[predict];
                DataPointSpawner.Instance.DataPoints.Enqueue(datapoint);
            }
        }

        public double[][] LoadData()
        {
            //ändra till filips läsdata första array är hur många och sista är värden dvs
            // [10] == 10 styckna linjer
            //data[0] = new double[] {2.0, 4,0, 0} 2.0=x 4.0=y 0=lable eller class. Detta är första linjen i datasettet tar endast in 2 featuers
            //tar endast in nummer, förändra ickenummer data till nummer eller ta bort.

            double[][] data = new double[3][];
            data[0] = new double[] { 5.0, 5.0, 4.4, 1 };
            data[1] = new double[] { 2.0, 4.0, 2.0, 0 };
            data[2] = new double[] { 1.0, 1.0, 3.0, 2 };

            // skapa unity grejorna här

            return data;
        }
    }
}

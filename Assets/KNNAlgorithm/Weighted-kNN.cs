using System;
using System.Collections.Generic;
using System.Text;

namespace KnnConsoleAppForUnity
{
    public class Weighted_kNN
    {
        public void Analyze( double[] unknown, double[][] data, int k, int c)
        {
            //räkna distanserna mellan punkter
            int N = data.Length;
            double[] distances = new double[N];
            for(int i = 0; i < N; i++)
            {
                distances[i] = DistFunc(unknown, data[i]);
            }
            // få fram ordern
            int[] ordering = new int[N];
            for(int i =0; i < N; i++)
            {
                ordering[i] = i;
            }
            double[] distancesCopy = new double[N];
            Array.Copy(distances, distancesCopy, distances.Length);
            Array.Sort(distancesCopy, ordering);
            double[] kNearestDists = new double[k];
            for(int i= 0; i< k; i++)
            {
                int idx = ordering[i];
                Show(data[idx]);
                Console.WriteLine("     distamce=" + distances[idx].ToString("F4"));
                kNearestDists[i] = distances[idx];
            }
            // vote
            double[] votes = new double[c]; // en per lable/class
            double[] wts = MakeWeights(k, kNearestDists);
            Console.WriteLine("Weights(inverse technique):  ");
            for(int i = 0; i < wts.Length; i++)
            {
                Console.WriteLine(wts[i].ToString("F4") + "  ");
            }
            Console.WriteLine("\n\nPredicted class: ");
            for( int i= 0; i< k; i++)
            {
                int idx = ordering[i];
                int predClass = (int)data[idx][3]; //hämta ut lable/class ändra 3an om fel plats
                votes[predClass] += wts[i] * 1.0;
            }
            double predictmaxWeight = 0.0; //hitta störta vikten.
            int predclass = 0;

            for(int i = 0; i < c; i++)
            {
                Console.WriteLine("[" + i + "]  " + votes[i].ToString("F4"));
                if (votes[i] > predictmaxWeight)
                { 
                    predictmaxWeight = votes[i];
                    predclass = i;
                }
            }
            Console.WriteLine("The predicted class is = " + predclass +" with the weight of: " +  predictmaxWeight);
        }
        public double[] MakeWeights(int k, double[] distances)
        {
            // inverse technique
            double[] result = new double[k]; // en per granne ;DD
            double sum = 0.0;
            for(int i = 0; i < k; i++)
            {
                result[i] = 1.0 / distances[i];
                sum += result[i];
            }
            for( int i = 0; i < k; i++)
            {
                result[i] /= sum;
            }
            return result;
        }
        public double DistFunc(double[] unknown, double[] dataPoint)
        {
            double sum = 0.0;
            for( int i = 0; i<2; i++)
            {
                double diff = unknown[i] - dataPoint[i + 1];
                sum += Math.Pow(diff, 2);
            }
            return Math.Sqrt(sum);
        }
        public void Show(double[] v)
        {
            Console.Write("idx = " + v[0].ToString().PadLeft(3) + "  (" + v[1].ToString("F2") + " " +v[2].ToString("F2") + ") class = " + v[3]);
        }
        public double[][] loadData()
        {
            //lägg till filips ladda datta sätt i dubbelarray
            double[][] data = new double[33][];
            data[0] = new double[] { 0, 0.25, 0.43, 0 };
            data[1] = new double[] { 1, 0.26, 0.54, 0 };
            data[2] = new double[] { 2, 0.27, 0.60, 0 };
            data[3] = new double[] { 3, 0.37, 0.31, 0 };
            data[4] = new double[] { 4, 0.38, 0.70, 0 };
            data[5] = new double[] { 5, 0.49, 0.32, 0 };
            data[6] = new double[] { 6, 0.46, 0.70, 0 };
            data[7] = new double[] { 7, 0.55, 0.32, 0 };
            data[8] = new double[] { 8, 0.58, 0.74, 0 };
            data[9] = new double[] { 9, 0.67, 0.42, 0 };
            data[10] = new double[] { 10, 0.69, 0.51, 0 };
            data[11] = new double[] { 11, 0.66, 0.63, 0 };
            data[12] = new double[] { 12, 0.29, 0.43, 1 };
            data[13] = new double[] { 13, 0.35, 0.51, 1 };
            data[14] = new double[] { 14, 0.39, 0.63, 1 };
            data[15] = new double[] { 15, 0.47, 0.40, 1 };
            data[16] = new double[] { 16, 0.48, 0.50, 1 };
            data[17] = new double[] { 17, 0.45, 0.61, 1 };
            data[18] = new double[] { 18, 0.55, 0.41, 1 };
            data[19] = new double[] { 19, 0.57, 0.53, 1 };
            data[20] = new double[] { 20, 0.56, 0.62, 1 };
            data[21] = new double[] { 21, 0.68, 0.12, 1 };
            data[22] = new double[] { 22, 0.78, 0.24, 1 };
            data[23] = new double[] { 23, 0.86, 0.21, 1 };
            data[24] = new double[] { 24, 0.86, 0.22, 1 };
            data[25] = new double[] { 25, 0.86, 0.12, 1 };
            data[26] = new double[] { 26, 0.78, 0.14, 1 };
            data[27] = new double[] { 27, 0.28, 0.13, 2 };
            data[28] = new double[] { 28, 0.25, 0.21, 2 };
            data[29] = new double[] { 29, 0.39, 0.14, 2 };
            data[30] = new double[] { 30, 0.37, 0.24, 2 };
            data[31] = new double[] { 31, 0.45, 0.13, 2 };
            data[32] = new double[] { 32, 0.46, 0.22, 2 };

            return data;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace KnnConsoleAppForUnity
{
    public class Weighted_kNN // Gammal kod?
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
    }
}

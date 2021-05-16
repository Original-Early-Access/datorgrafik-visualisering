using System;
using System.Collections.Generic;
using System.Text;


namespace KnnConsoleAppForUnity
{
    public static class kNN
    {
        public static int Classify(double[] unknown, double[][] trainData, int numClasses, int k, int numfeatures) // classifyer för knn
        {
            int n = trainData.Length;
            IndexAndDistance[] info = new IndexAndDistance[n];

            for(int i = 0; i < n; i++)
            {
                IndexAndDistance curr = new IndexAndDistance();
                double dist = Distance(unknown, trainData[i]);
                curr.idx = i;
                curr.dist = dist;
                info[i] = curr;
            }

            Array.Sort(info); //sortera distans

            /*for(int i = 0; i<k; i++)
            {
                int c = (int)trainData[info[i].idx][numfeatures]; // tar bort lable eller class ändras om det finns fler featuers
                string dist = info[i].dist.ToString();
            }*/

            int result = Vote(info, trainData, numClasses, k, numfeatures);
            return result;
        }

        static int Vote(IndexAndDistance[] info, double[][] trainData, int numClasses, int k, int numfeatures)
        {
            int[] votes = new int[numClasses]; // tar inte hand om dubbel data. Ta bort dubbeldata!
            for (int i =0; i < k; i++)
            {
                int idx = info[i].idx;
                int c = (int)trainData[idx][numfeatures]; // tar ut lables ändras om det finns fler featurs
                votes[c]++;
            }
            int mostVotes = 0;
            int classWithMostVotes = 0;
            for( int j = 0; j< numClasses; j++)
            {
                if (votes[j] > mostVotes)
                {
                    mostVotes = votes[j];
                    classWithMostVotes = j;
                }
            }
            return classWithMostVotes; //returnerar den klassen som har fått mest röster dvs den som är mest runt om unknown
        } 

        static double Distance(double[] unknown, double[] data)
        {
            double sum = 0.0;
            for(int i = 0; i<unknown.Length; i++)
            {
                sum += Math.Pow((unknown[i] - data[i]), 2);
            }
            return Math.Sqrt(sum);
        }
    }
}

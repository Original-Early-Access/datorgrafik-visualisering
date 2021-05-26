using Assets.KNNAlgorithm;
using System;
using System.Collections.Generic;
using System.Text;


namespace KnnConsoleAppForUnity
{
    public static class kNN
    {
        public static int Classify(double[] unknown, DataSet trainData, int k)
        {
            int rowCount = trainData.DataRows.Count;
            IndexAndDistance[] info = new IndexAndDistance[rowCount];

            for(int i = 0; i < rowCount; i++)
            {
                IndexAndDistance curr = new IndexAndDistance();
                double dist = Distance(unknown, trainData.DataRows[i].Values.ToArray());
                curr.idx = i;
                curr.dist = dist;
                info[i] = curr;
            }

            Array.Sort(info); //sortera distans
            return Vote(info, trainData, k);
        }

        static int Vote(IndexAndDistance[] info, DataSet trainData, int k)
        {
            int[] votes = new int[trainData.Labels.Count]; // tar inte hand om dubbel data. Ta bort dubbeldata!

            for (int i =0; i < k; i++)
                votes[trainData.DataRows[info[i].idx].LabelID]++;

            int mostVotes = 0;
            int classWithMostVotes = 0;
            for( int j = 0; j < trainData.Labels.Count; j++)
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

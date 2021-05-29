using Assets.KNNAlgorithm;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace KnnConsoleAppForUnity
{
    public static class kNN
    {
        public static int Classify(DataRow dataRow, DataSet trainData, int k)
        {
            int rowCount = trainData.DataRows.Count;
            IndexAndDistance[] info = new IndexAndDistance[rowCount];

            for(int i = 0; i < rowCount; i++)
            {
                IndexAndDistance curr = new IndexAndDistance();
                double dist = Distance(dataRow, trainData.DataRows[i].AllValues.ToArray());
                curr.idx = i;
                curr.dist = dist;
                info[i] = curr;
            }
            
            Array.Sort(info); //sortera distans
            return Vote(info, trainData, k);
        }

        public static double Regressor(DataRow dataRow, DataSet trainData, int k)
        {
            int rowCount = trainData.DataRows.Count;
            IndexAndDistance[] info = new IndexAndDistance[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                IndexAndDistance curr = new IndexAndDistance();
                double dist = Distance(dataRow, trainData.DataRows[i].AllValues.ToArray());
                curr.idx = i;
                curr.dist = dist;
                info[i] = curr;
            }

            Array.Sort(info); //sortera distans
            return Regress(info, trainData, k);
        }
        static int Vote(IndexAndDistance[] info, DataSet trainData, int k)
        {
            int[] votes = new int[trainData.Labels.Count]; // tar inte hand om dubbel data. Ta bort dubbeldata!

            for (int i = 0; i < k; i++)
            {
                int idx = info[i].idx;
                int c = trainData.DataRows.ToArray()[idx].LabelID;
                votes[c]++;
            }


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

        static double Regress(IndexAndDistance[] info, DataSet trainData, int k)
        {
            double sum = 0;

            for (int i = 0; i < k; i++)
            {
                int idx = info[i].idx;
                sum += trainData.DataRows.ToArray()[idx].LabelID;
            }
            if (sum == 0 || k == 0)
                return 0;
            return sum /= k;
        }



        static double Distance(DataRow dataRow, double[] data)
        {
            double sum = 0.0;

            for(int i = 0; i < dataRow.FeatureIDs.Count; i++)
                sum += Math.Pow(dataRow.kNNValues[i] - data[dataRow.FeatureIDs[i]], 2);

            return Math.Sqrt(sum);
        }

        public static int WeightedClassify(DataRow dataRow, DataSet trainData, int k)
        {
            int rowCount = trainData.DataRows.Count;
            IndexAndDistance[] info = new IndexAndDistance[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                IndexAndDistance curr = new IndexAndDistance();
                double dist = Distance(dataRow, trainData.DataRows[i].AllValues.ToArray());
                curr.idx = i;
                curr.dist = dist;
                info[i] = curr;
            }

            Array.Sort(info); //sortera distans
            double[] weights = MakeWeights(k, info);

            return WeightedVote(info, trainData, weights, k);
        }
        public static double WeightedRegressedClassify(DataRow dataRow, DataSet trainData, int k)
        {
            int rowCount = trainData.DataRows.Count;
            IndexAndDistance[] info = new IndexAndDistance[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                IndexAndDistance curr = new IndexAndDistance();
                double dist = Distance(dataRow, trainData.DataRows[i].AllValues.ToArray());
                curr.idx = i;
                curr.dist = dist;
                info[i] = curr;
            }

            Array.Sort(info); //sortera distans
            double[] weights = MakeWeights(k, info);

            return WeightedRegressedVote(info, trainData, weights, k);
        }

        static int WeightedVote(IndexAndDistance[] info, DataSet trainData,double[] weights,  int k)
        {
            double[] votes = new double[trainData.Labels.Count]; // en per lable/class

            for (int i = 0; i < k; i++)
            {
                int idx = info[i].idx;
                int predClass = trainData.DataRows.ToArray()[idx].LabelID;
                votes[predClass] += weights[i] * 1.0;
            }
            double predictmaxWeight = 0.0; //hitta störta vikten.
            int predclass = 0;


            for (int i = 0; i < trainData.Labels.Count; i++)
            {
                if (votes[i] > predictmaxWeight)
                {
                    predictmaxWeight = votes[i];
                    predclass = i;
                }
            }
            return predclass;
        }
        static double WeightedRegressedVote(IndexAndDistance[] info, DataSet trainData, double[] weights, int k)
        {
            double sum = 0;
            for (int i = 0; i < k; i++)
            {
                int idx = info[i].idx;
                sum += trainData.DataRows.ToArray()[idx].LabelID + weights[i];
            }
            if (sum == 0 || k == 0)
                return 0;
            return sum / k;
        }
        public static double[] MakeWeights(int k, IndexAndDistance[] info)
        {
            // inverse technique
            double[] result = new double[k]; // en per granne ;DD
            double sum = 0.0;
            for (int i = 0; i < k; i++)
            {
                result[i] = 1.0 / info[i].dist;
                sum += result[i];
            }
            for (int i = 0; i < k; i++)
            {
                result[i] /= sum;
            }
            return result;
        }
    }
}

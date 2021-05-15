﻿using System;
using System.Collections.Generic;
using System.Text;


namespace KnnConsoleAppForUnity
{
    public class kNN
    {
        public int Classify(double[] unknown, double[][] trainData, int numClasses, int k, int numfeatures) // classifyer för knn
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

            for(int i = 0; i<k; i++)
            {
                int c = (int)trainData[info[i].idx][numfeatures]; // tar bort lable eller class ändras om det finns fler featuers
                string dist = info[i].dist.ToString();
            }

            int result = Vote(info, trainData, numClasses, k, numfeatures);
            return result;
        }

        int Vote(IndexAndDistance[] info, double[][] trainData, int numClasses, int k, int numfeatures)
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

        double Distance(double[] unknown, double[] data)
        {
            double sum = 0.0;
            for(int i = 0; i<unknown.Length; i++)
            {
                sum += Math.Pow((unknown[i] - data[i]), 2);
            }
            return Math.Sqrt(sum);
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
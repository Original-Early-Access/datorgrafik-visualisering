using KnnConsoleAppForUnity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace Assets.KNNAlgorithm
{
    public class KNNController
    {

        private static readonly Lazy<KNNController> lazy = new Lazy<KNNController>(() => new KNNController());
        public static KNNController Instance { get { return lazy.Value; } }

        public double[][] TrainingData { get; set; }
        public double[][] UnkownData { get; set; }
        public int NumFeatures { get; set; }
        public List<string> Labels { get; set; } = new List<string>();
        public List<string> FeatureNames { get; set; } = new List<string>();
        public void StartPrediction()
        {
            kNN kNN = new kNN();

            LoadData();

            List<DataPoint> dataPoints = new List<DataPoint>();
            foreach (var value in UnkownData)
            {
                if(value != null)
                    dataPoints.Add(new DataPoint()
                    {
                        X = (float)value[0],
                        Y = (float)value[1],
                        Z = (float)value[2],
                    });
            }
                

            int k = 1;
            foreach(var datapoint in dataPoints)
            {
                
                int predict = kNN.Classify(new double[3] { datapoint.X, datapoint.Y, datapoint.Z }, TrainingData, Labels.Count(), k, NumFeatures);
                datapoint.Label = Labels[predict];
                datapoint.LabelID = predict;
                DataPointSpawner.Instance.DataPoints.Enqueue(datapoint);
            }
        }

        public void LoadData()
        {
            List<string[]> data = LoadCSV(@"C:\Users\Invurnable\source\repos\Teafuu\datorgrafik-visualisering\Iris.csv");
            int unknowLenght = (data.Count() - 1) / 2;

            FeatureNames = data[0].ToList();
            data.RemoveAt(0);
            Randomize(data);

            List<string[]> trainingData = new List<string[]>();

            for(int i=1; i < unknowLenght; i++)
                trainingData.Add(data[i]);
            

            NumFeatures = data[0].Count() - 2;
            TrainingData = FillData(trainingData);
            UnkownData = FillData(data);
        }

        private double[][] FillData(List<string[]> data)
        {
            double[][] newData = new double[data.Count() - 1][];

            for (int i = 1; i < data.Count(); i++)
            {
                double[] line = new double[data[i].Count() - 1];
                int j;

                for (j = 1; j < data[i].Count() - 1; j++)
                    line[j - 1] = double.Parse(data[i][j], CultureInfo.InvariantCulture);
                
                if (!Labels.Contains(data[i][j]))
                    Labels.Add(data[i][j]);
                line[j - 1] = Labels.IndexOf(data[i][j]);
                newData[i - 1] = line;
            }
            return newData;
        }

        public List<string[]> LoadCSV(string filename)
        {
            List<string[]> data = new List<string[]>();
            if (File.Exists(filename))
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                        data.Add(CSVParser.Split(line));
                    }
                }
            }
            return data;
        }
        public static void Randomize<T>(List<T> items)
        {
            Random rand = new Random();

            // For each spot in the array, pick
            // a random item to swap into that spot.
            for (int i = 0; i < items.Count - 1; i++)
            {
                int j = rand.Next(i, items.Count);
                T temp = items[i];
                items[i] = items[j];
                items[j] = temp;
            }
        }
    }
}

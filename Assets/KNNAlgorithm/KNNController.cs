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
            List<string[]> data = LoadCSV(@"D:\Högskolan\År 2\Unity\Original-Early-Access\datorgrafik-visualisering\Iris.csv");
            int unknowLenght = (data.Count() - 1) / 2;
            List<string[]> unknownData = new List<string[]>();
            List<string[]> trainingData = new List<string[]>();

            for(int i=1; i < unknowLenght; i++)
                unknownData.Add(data[i]);

            for (int i = unknowLenght; i < data.Count(); i++)
                trainingData.Add(data[i]);

            TrainingData = new double[trainingData.Count()-1][];
            UnkownData = new double[unknownData.Count()-1][];

            NumFeatures = data[0].Count() - 2;
            for (int i = 1; i < trainingData.Count(); i++)
            {
                double[] line = new double[trainingData[i].Count()];
                int j;
                for (j = 1; j < trainingData[i].Count() - 1; j++)
                {
                    Debug.Log(trainingData[i][j]);
                    line[j - 1] = double.Parse(trainingData[i][j], CultureInfo.InvariantCulture);
                }
                Debug.Log(trainingData[i][j]);
                if (!Labels.Contains(trainingData[i][j]))
                    Labels.Add(trainingData[i][j]);

                line[j] = Labels.IndexOf(trainingData[i][j]);
                TrainingData[i - 1] = line;
            }

            for (int i = 1; i < unknownData.Count(); i++)
            {
                double[] line = new double[unknownData[i].Count()];
                int j;
                for (j = 1; j < unknownData[i].Count() - 1; j++)
                {
                    line[j - 1] = double.Parse(unknownData[i][j], CultureInfo.InvariantCulture);
                }
                if (!Labels.Contains(unknownData[i][j]))
                    Labels.Add(unknownData[i][j]);

                line[j] = Labels.IndexOf(unknownData[i][j]);
                UnkownData[i - 1] = line;
            }
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
    }
}

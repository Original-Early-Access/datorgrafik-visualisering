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

        public DataSet TrainingData { get; set; }
        public DataSet UnkownData { get; set; }
        public List<string[]> data { get; set; } = new List<string[]>();
        public List<string> Labels { get; set; } = new List<string>();
        public List<string> FeatureNames { get; set; } = new List<string>();
        public List<int> SelectedFeatures { get; set; } = new List<int>();
        public void StartPrediction()
        {
            PrepareData();
            List<DataPoint> dataPoints = new List<DataPoint>();

            foreach (var value in UnkownData.DataRows)
            {
                if (value != null)

                    dataPoints.Add(new DataPoint()
                    {
                        LabelID = value.LabelID,
                        Label = Labels[value.LabelID],
                        X = (float)value.Values[SelectedFeatures[0]],
                        Y = (float)value.Values[SelectedFeatures[1]],
                        Z = (float)value.Values[SelectedFeatures[2]],
                    });
            }

            DataPointSpawner.Instance.Destroy = true;
            dataPoints.ForEach(d => RunPrediction(d));
        }

        public void LoadData(string csv = @"D:\Högskolan\År 2\Unity\Original-Early-Access\datorgrafik-visualisering\Iris.csv")
        {
            data = LoadCSV(csv);
            FeatureNames = data[0].ToList();
            data.RemoveAt(0);
        }

        public void PrepareData()
        {
            List<string[]> trainingData = new List<string[]>();

            int unknowLenght = (data.Count() - 1) / 2;
            Randomize(data);

            for (int i=1; i < unknowLenght; i++)
                trainingData.Add(data[i]);

            TrainingData = FillData(trainingData);
            UnkownData = FillData(data);
            TrainingData.Labels = Labels;
            UnkownData.Labels = Labels;
        }

        private DataSet FillData(List<string[]> data)
        {
            DataSet newData = new DataSet();

            for (int i = 1; i < data.Count; i++)
            {
                int labelPos = data[i].Count() - 1;
                if (!Labels.Contains(data[i][labelPos]))
                    Labels.Add(data[i][labelPos]);

                newData.DataRows.Add(new DataRow()
                {
                    Values = data[i].Select(value => double.Parse(value, CultureInfo.InvariantCulture)).ToList(),
                    LabelID = Labels.IndexOf(data[i][labelPos])
                });
            }
            return newData;
        }

        public void RunPrediction(DataPoint dataPoint, int k = 1)
        {
            int predict = kNN.Classify(new double[3] { dataPoint.X, dataPoint.Y, dataPoint.Z }, TrainingData, k);
            dataPoint.Label = Labels[predict];
            dataPoint.LabelID = predict;
            DataPointSpawner.Instance.DataPoints.Enqueue(dataPoint);
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

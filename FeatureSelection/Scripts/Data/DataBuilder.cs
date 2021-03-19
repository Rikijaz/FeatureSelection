#region

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

#endregion

namespace FeatureSelection.Scripts.Data
{
	public static class DataBuilder
	{
		public static (Dictionary<uint, List<Datum>> smallDataByFeature,
			Dictionary<uint, List<Datum>> largeDataByFeature)
			BuildAllData()
		{
			Dictionary<uint, List<Datum>> smallDataByFeature =
				BuildData(@"../../Resources/large_data.txt");

			Dictionary<uint, List<Datum>> largeDataByFeature =
				BuildData(@"../../Resources/large_data.txt");

			return (smallDataByFeature, largeDataByFeature);
		}

		private static Dictionary<uint, List<Datum>> BuildData(string dataFilePath)
		{
			string[] lines = File.ReadAllLines(dataFilePath);

			List<(double, Dictionary<uint, double>)> featureValuesByClassAndFeature =
				new List<(double, Dictionary<uint, double>)>();

			for (uint i = 0; i < lines.Length; i++)
			{
				(double classValue, Dictionary<uint, double> featureValuesByFeature) =
					ParseData(lines[i]);

				featureValuesByClassAndFeature.Add((classValue, featureValuesByFeature));
			}

			int featureCount = featureValuesByClassAndFeature.First().Item2.Count;

			Dictionary<uint, List<Datum>> dataByFeature = new Dictionary<uint, List<Datum>>();

			for (uint i = 1; i <= featureCount; ++i)
			{
				dataByFeature.Add(i, new List<Datum>());
			}

			for (int i = 0; i < featureValuesByClassAndFeature.Count; i++)
			{
				(double classValue, Dictionary<uint, double> featureValuesByFeature) =
					featureValuesByClassAndFeature[i];

				foreach (KeyValuePair<uint, double> keyValuePair in featureValuesByFeature)
				{
					uint feature = keyValuePair.Key;
					double featureValue = keyValuePair.Value;
					Datum datum = new Datum(classValue, feature, featureValue);
					dataByFeature[feature].Add(datum);
				}
			}

			return dataByFeature;
		}

		private static (double classValue, Dictionary<uint, double> valuesByFeature) ParseData(
			string line)
		{
			string[] words = Regex.Split(line, @"[\s]+");

			double classValue = ParseValue(words[1]);

			Dictionary<uint, double> featureValuesByFeature = new Dictionary<uint, double>();

			for (uint j = 2; j < words.Length; j++)
			{
				double featureValue = ParseValue(words[j]);
				featureValuesByFeature.Add(j - 1, featureValue);
			}

			return (classValue, featureValuesByFeature);
		}

		private static double ParseValue(string word) => double.Parse(word, NumberStyles.Float);
	}
}
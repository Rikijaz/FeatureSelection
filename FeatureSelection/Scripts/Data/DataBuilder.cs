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
		public static (Dictionary<uint, Dictionary<uint, Datum>> smallDataByFeature,
			Dictionary<uint, Dictionary<uint, Datum>> largeDataByFeature)
			BuildAllData()
		{
			Dictionary<uint, Dictionary<uint, Datum>> smallDataByFeature =
				BuildData(@"../../Resources/small_data.txt");

			Dictionary<uint, Dictionary<uint, Datum>> largeDataByFeature =
				BuildData(@"../../Resources/large_data.txt");

			return (smallDataByFeature, largeDataByFeature);
		}

		private static Dictionary<uint, Dictionary<uint, Datum>> BuildData(string dataFilePath)
		{
			string[] lines = File.ReadAllLines(dataFilePath);

			List<(uint, double, Dictionary<uint, double>)> featureValuesByClassAndFeature =
				new List<(uint, double, Dictionary<uint, double>)>();

			uint id = 0;

			for (uint i = 0; i < lines.Length; i++)
			{
				(double classValue, Dictionary<uint, double> featureValuesByFeature) =
					ParseData(lines[i]);

				featureValuesByClassAndFeature.Add((id, classValue, featureValuesByFeature));

				id++;
			}

			int featureCount = featureValuesByClassAndFeature.First().Item3.Count;

			Dictionary<uint, Dictionary<uint, Datum>> dataByFeatureAndId =
				new Dictionary<uint, Dictionary<uint, Datum>>();

			for (uint i = 1; i <= featureCount; ++i)
			{
				dataByFeatureAndId.Add(i, new Dictionary<uint, Datum>());
			}

			for (int i = 0; i < featureValuesByClassAndFeature.Count; i++)
			{
				(uint idValue, double classValue, Dictionary<uint, double> featureValuesByFeature) =
					featureValuesByClassAndFeature[i];

				foreach (KeyValuePair<uint, double> keyValuePair in featureValuesByFeature)
				{
					uint feature = keyValuePair.Key;
					double featureValue = keyValuePair.Value;
					Datum datum = new Datum(idValue, classValue, feature, featureValue);
					dataByFeatureAndId[feature].Add(idValue, datum);
				}
			}

			return dataByFeatureAndId;
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
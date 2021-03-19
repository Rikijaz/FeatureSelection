#region

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

#endregion

namespace FeatureSelection.Scripts.Data
{
	public static class DataBuilder
	{
		public static (IReadOnlyList<Datum> smallData, IReadOnlyList<Datum> largeData)
			BuildAllData()
		{
			List<Datum> smallData = BuildData(@"../../Resources/large_data.txt");
			List<Datum> largeData = BuildData(@"../../Resources/large_data.txt");

			return (smallData, largeData);
		}

		private static List<Datum> BuildData(string dataFilePath)
		{
			string[] lines = File.ReadAllLines(dataFilePath);

			List<Datum> data = new List<Datum>();

			for (uint i = 0; i < lines.Length; i++)
			{
				Datum datum = BuildDatum(lines[i]);
				data.Add(datum);
			}

			return data;
		}

		private static Datum BuildDatum(string line)
		{
			string[] words = Regex.Split(line, @"[\s]+");

			double classValue = ParseValue(words[1]);

			Dictionary<uint, double> valuesByFeature = new Dictionary<uint, double>();

			for (uint j = 2; j < words.Length; j++)
			{
				double featureValue = ParseValue(words[j]);
				valuesByFeature.Add(j - 1, featureValue);
			}

			return new Datum(classValue, valuesByFeature);
		}

		private static double ParseValue(string word) => double.Parse(word, NumberStyles.Float);
	}
}
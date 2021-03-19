#region

using System;
using System.Collections.Generic;
using FeatureSelection.Scripts.Data;

#endregion

namespace FeatureSelection.Scripts
{
	internal static class Program
	{
		public static void Main(string[] args)
		{
			(Dictionary<uint, List<Datum>> smallDataByFeature,
				Dictionary<uint, List<Datum>> largeDataByFeature) = DataBuilder.BuildAllData();

			foreach (KeyValuePair<uint, List<Datum>> pair in smallDataByFeature)
			{
				List<Datum> data = pair.Value;

				Console.WriteLine($"Feature '{pair.Key}'");

				if (pair.Key >= 10)
				{
					return;
				}

				for (int i = 0; i < data.Count; i++)
				{
					Console.WriteLine(
						$"Class: '{data[i].ClassValue}' Feature: '{data[i].Feature}' FeatureValue: '{data[i].FeatureValue}'");
				}
			}
		}
	}
}
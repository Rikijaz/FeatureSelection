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

			Search(smallDataByFeature);

			// foreach (KeyValuePair<uint, List<Datum>> pair in smallDataByFeature)
			// {
			// 	List<Datum> data = pair.Value;
			//
			// 	Console.WriteLine($"Feature '{pair.Key}'");
			//
			// 	if (pair.Key >= 10)
			// 	{
			// 		return;
			// 	}
			//
			// 	for (int i = 0; i < data.Count; i++)
			// 	{
			// 		Console.WriteLine(
			// 			$"Class: '{data[i].ClassValue}' Feature: '{data[i].Feature}' FeatureValue: '{data[i].FeatureValue}'");
			// 	}
			// }
		}

		private static void Search(Dictionary<uint, List<Datum>> dataByFeature)
		{
			HashSet<uint> unselectedFeatures = new HashSet<uint>(dataByFeature.Keys);

			HashSet<uint> selectedFeatures = new HashSet<uint>();

			uint level = 1;

			while (unselectedFeatures.Count > 0)
			{
				Console.WriteLine($"Traversing search tree level '{level}' ...");
				uint selectedFeature = 0;
				float selectedFeatureAccuracy = 0;

				foreach (uint unselectedFeature in unselectedFeatures)
				{
					Console.WriteLine($"Considering adding feature '{unselectedFeature}' ...");

					float accuracy =
						CalculateCrossValidationAccuracy(dataByFeature[unselectedFeature]);

					if (accuracy >= selectedFeatureAccuracy)
					{
						selectedFeatureAccuracy = accuracy;
						selectedFeature = unselectedFeature;
					}
				}

				Console.WriteLine(
					$"At search tree level '{level}', added feature '{selectedFeature}'.");

				unselectedFeatures.Remove(selectedFeature);
				selectedFeatures.Add(selectedFeature);
				level++;
			}

			uint index = 1;

			foreach (uint selectedFeature in selectedFeatures)
			{
				Console.WriteLine($"{index}. {selectedFeature}.");
				index++;
			}
		}

		private static float CalculateCrossValidationAccuracy(List<Datum> data)
		{
			const double range = float.MaxValue - (double) float.MinValue;
			double value = new Random().NextDouble();

			return (float) ((value * range) + float.MinValue);
		}
	}
}
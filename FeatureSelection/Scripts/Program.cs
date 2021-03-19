#region

using System;
using System.Collections.Generic;
using System.Linq;
using FeatureSelection.Scripts.Data;

#endregion

namespace FeatureSelection.Scripts
{
	public static class LogUtility
	{
		private const LogLevel Level = LogLevel.Info;

		public static void Log(object message, LogLevel logLevel)
		{
			if (logLevel >= Level)
			{
				Console.WriteLine(message);
			}
		}
	}

	internal static class Program
	{
		public static void Main(string[] args)
		{
			(Dictionary<uint, Dictionary<uint, Datum>> smallDataByFeatureAndId,
					Dictionary<uint, Dictionary<uint, Datum>> largeDataByFeatureAndId) =
				DataBuilder.BuildAllData();

			Search(largeDataByFeatureAndId);
		}

		private static void Search(Dictionary<uint, Dictionary<uint, Datum>> dataByFeatureAndId)
		{
			HashSet<uint> unselectedFeatures = new HashSet<uint>(dataByFeatureAndId.Keys);

			HashSet<uint> selectedFeatures = new HashSet<uint>();

			uint level = 1;

			while (unselectedFeatures.Count > 0)
			{
				LogUtility.Log($"Traversing search tree level '{level}' ...", LogLevel.Trace);
				uint selectedFeature = 0;
				float selectedFeatureAccuracy = 0;

				foreach (uint unselectedFeature in unselectedFeatures)
				{
					LogUtility.Log(
						$"Considering adding feature '{unselectedFeature}' ...",
						LogLevel.Trace);

					float accuracy =
						CalculateCrossValidationAccuracy(
							dataByFeatureAndId,
							selectedFeatures,
							unselectedFeature);

					LogUtility.Log($"Feature accuracy '{accuracy}'.", LogLevel.Trace);

					if (accuracy >= selectedFeatureAccuracy)
					{
						selectedFeatureAccuracy = accuracy;
						selectedFeature = unselectedFeature;
					}
				}

				LogUtility.Log(
					$"At search tree level '{level}', added feature '{selectedFeature} with accuracy {selectedFeatureAccuracy}'.",
					LogLevel.Info);

				unselectedFeatures.Remove(selectedFeature);
				selectedFeatures.Add(selectedFeature);
				level++;

				uint index = 1;

				foreach (uint feature in selectedFeatures)
				{
					LogUtility.Log($"{index}. {feature}.", LogLevel.Info);
					index++;
				}
			}
		}

		private static float CalculateCrossValidationAccuracy(
			Dictionary<uint, Dictionary<uint, Datum>> dataByFeatureAndId,
			IEnumerable<uint> selectedFeatures,
			uint featureToAdd)
		{
			uint[] features =
				new uint[] { featureToAdd }.ToArray().Concat(selectedFeatures).ToArray();

			uint[] ids = dataByFeatureAndId.First().Value.Keys.ToArray();

			uint successfullyClassifiedIdCount = 0;

			for (uint i = 0; i < ids.Length; i++)
			{
				uint currentId = ids[i];

				if (ClassifyNeighbors(currentId, ids, dataByFeatureAndId, features))
				{
					successfullyClassifiedIdCount++;
				}
			}

			return (float) successfullyClassifiedIdCount / ids.Length;
		}

		private static bool ClassifyNeighbors(
			uint currentId,
			IReadOnlyList<uint> ids,
			Dictionary<uint, Dictionary<uint, Datum>> dataByFeatureAndId,
			IReadOnlyList<uint> features)
		{
			uint nearestNeighborId = 0;
			double lowestDistance = double.MaxValue;

			for (int i = 0; i < ids.Count; i++)
			{
				uint id = ids[i];

				if (id == currentId)
				{
					continue;
				}

				List<double> differenceValues = new List<double>();

				for (int j = 0; j < features.Count; j++)
				{
					uint feature = features[j];

					Dictionary<uint, Datum> dataById = dataByFeatureAndId[feature];

					double difference =
						dataById[currentId].FeatureValue - dataById[id].FeatureValue;

					differenceValues.Add(difference);
				}

				double distance = CalculateDistance(differenceValues);

				if (distance <= lowestDistance)
				{
					lowestDistance = distance;
					nearestNeighborId = id;
				}
			}

			Dictionary<uint, Datum> firstDataById = dataByFeatureAndId.First().Value;
			IDatumIdentification currentDatumIdentification = firstDataById[currentId];

			IDatumIdentification nearestNeighborDatumIdentification =
				firstDataById[nearestNeighborId];

			return Math.Abs(
					nearestNeighborDatumIdentification.ClassValue -
					currentDatumIdentification.ClassValue) <
				0.001f;
		}

		private static double CalculateDistance(IList<double> differenceValues)
		{
			double powerSum = 0f;

			for (int i = 0; i < differenceValues.Count; i++)
			{
				powerSum += Math.Pow(differenceValues[i], 2);
			}

			return Math.Sqrt(powerSum);
		}
	}
}
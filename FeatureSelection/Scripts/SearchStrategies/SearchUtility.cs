#region

using System;
using System.Collections.Generic;
using System.Linq;
using FeatureSelection.Scripts.Data;

#endregion

namespace FeatureSelection.Scripts.SearchStrategies
{
	public static class SearchUtility
	{
		public static float CrossValidateAccuracy(
			Dictionary<uint, Dictionary<uint, Datum>> dataByFeatureAndId,
			uint[] features)
		{
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
			IReadOnlyDictionary<uint, Dictionary<uint, Datum>> dataByFeatureAndId,
			IReadOnlyList<uint> features)
		{
			uint nearestNeighborId = 0;
			double lowestDistance = double.MaxValue;

			for (int i = 0; i < ids.Count; i++)
			{
				uint compareId = ids[i];

				if (compareId == currentId)
				{
					continue;
				}

				double distance = CalculateDistance(
					currentId,
					compareId,
					dataByFeatureAndId,
					features);

				if (distance <= lowestDistance)
				{
					lowestDistance = distance;
					nearestNeighborId = compareId;
				}
			}

			Dictionary<uint, Datum> firstDataById = dataByFeatureAndId.First().Value;
			Datum currentDatum = firstDataById[currentId];

			Datum nearestNeighborDatum = firstDataById[nearestNeighborId];

			return Math.Abs(
					nearestNeighborDatum.ClassValue -
					currentDatum.ClassValue) <
				0.001f;
		}

		private static double CalculateDistance(
			uint currentId,
			uint compareId,
			IReadOnlyDictionary<uint, Dictionary<uint, Datum>> dataByFeatureAndId,
			IReadOnlyList<uint> features)
		{
			double powerSum = 0f;

			for (int j = 0; j < features.Count; j++)
			{
				uint feature = features[j];

				Dictionary<uint, Datum> dataById = dataByFeatureAndId[feature];

				Datum compareDatum = dataById[compareId];

				double difference =
					dataById[currentId].FeatureValue - compareDatum.FeatureValue;

				double powerSumDistance = difference * difference;

				powerSum += powerSumDistance;
			}

			double distance = Math.Sqrt(powerSum);

			return distance;
		}
	}
}
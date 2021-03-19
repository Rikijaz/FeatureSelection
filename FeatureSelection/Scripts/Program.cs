#region

using System.Collections.Generic;
using System.Linq;
using FeatureSelection.Scripts.Data;

#endregion

namespace FeatureSelection.Scripts
{
	internal static class Program
	{
		public static void Main(string[] args)
		{
			(Dictionary<uint, Dictionary<uint, Datum>> smallDataByFeatureAndId,
					Dictionary<uint, Dictionary<uint, Datum>> largeDataByFeatureAndId) =
				DataBuilder.BuildAllData();

			BackwardSearch(largeDataByFeatureAndId);
		}

		private static void ForwardSearch(
			Dictionary<uint, Dictionary<uint, Datum>> dataByFeatureAndId)
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

					uint[] features =
						new uint[] { unselectedFeature }.ToArray().
							Concat(selectedFeatures).
							ToArray();

					float accuracy = SearchUtility.CrossValidateAccuracy(
						dataByFeatureAndId,
						features);

					LogUtility.Log($"Feature accuracy '{accuracy}'.", LogLevel.Trace);

					if (accuracy >= selectedFeatureAccuracy)
					{
						selectedFeatureAccuracy = accuracy;
						selectedFeature = unselectedFeature;
					}
				}

				LogUtility.Log(
					$"At search tree level '{level}', added feature '{selectedFeature}' with accuracy '{selectedFeatureAccuracy}'.",
					LogLevel.Info);

				unselectedFeatures.Remove(selectedFeature);
				selectedFeatures.Add(selectedFeature);
				level++;
			}
		}

		private static void BackwardSearch(
			Dictionary<uint, Dictionary<uint, Datum>> dataByFeatureAndId)
		{
			HashSet<uint> availableFeatures = new HashSet<uint>(dataByFeatureAndId.Keys);

			uint level = 1;

			while (availableFeatures.Count > 0)
			{
				LogUtility.Log($"Traversing search tree level '{level}' ...", LogLevel.Trace);
				uint selectedFeature = 0;
				float selectedFeatureAccuracy = 1;

				foreach (uint currentFeature in availableFeatures)
				{
					LogUtility.Log(
						$"Considering removing feature '{currentFeature}' ...",
						LogLevel.Info);

					uint[] features = PrepareBackwardSearchFeatures(
						availableFeatures,
						currentFeature);

					float accuracy = SearchUtility.CrossValidateAccuracy(
						dataByFeatureAndId,
						features);

					LogUtility.Log($"Feature accuracy '{accuracy}'.", LogLevel.Info);

					if (accuracy <= selectedFeatureAccuracy)
					{
						selectedFeatureAccuracy = accuracy;
						selectedFeature = currentFeature;
					}
				}
				
				availableFeatures.Remove(selectedFeature);

				LogUtility.Log(
					$"At search tree level '{level}', removed feature '{selectedFeature}' resulting in accuracy '{selectedFeatureAccuracy}'.",
					LogLevel.Info);

				level++;
			}
		}

		private static uint[] PrepareBackwardSearchFeatures(
			IEnumerable<uint> availableFeatures,
			uint currentFeature)
		{
			List<uint> features = new List<uint>(availableFeatures);
			features.Remove(currentFeature);

			return features.ToArray();
		}
	}
}
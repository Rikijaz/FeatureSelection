#region

using System.Collections.Generic;
using System.Linq;
using FeatureSelection.Scripts.Data;
using FeatureSelection.Scripts.Log;

#endregion

namespace FeatureSelection.Scripts.SearchStrategies
{
	public class BackwardSearchStrategy : SearchStrategy
	{
		public static readonly BackwardSearchStrategy Instance = new BackwardSearchStrategy();

		private BackwardSearchStrategy()
		{
		}

		public override void Search(Dictionary<uint, Dictionary<uint, Datum>> dataByFeatureAndId)
		{
			HashSet<uint> availableFeatures = new HashSet<uint>(dataByFeatureAndId.Keys);

			uint level = 1;

			while (availableFeatures.Count > 0)
			{
				LogUtility.Log($"Traversing search tree level '{level}' ...", LogLevel.Trace);
				uint selectedFeature = 0;
				float selectedFeatureAccuracy = 0;

				foreach (uint currentFeature in availableFeatures)
				{
					LogUtility.Log(
						$"Considering removing feature '{currentFeature}' ...",
						LogLevel.Trace);

					uint[] features = PrepareFeatures(
						availableFeatures,
						currentFeature);

					float accuracy = SearchUtility.CrossValidateAccuracy(
						dataByFeatureAndId,
						features);

					if (accuracy >= selectedFeatureAccuracy)
					{
						selectedFeatureAccuracy = accuracy;
						selectedFeature = currentFeature;
					}
				}

				availableFeatures.Remove(selectedFeature);

				float newAccuracy = SearchUtility.CrossValidateAccuracy(
					dataByFeatureAndId,
					availableFeatures.ToArray());

				LogUtility.Log(
					$"At search tree level '{level}', removed feature '{selectedFeature}' resulting in accuracy '{newAccuracy}'.",
					LogLevel.Info);

				level++;
			}
		}

		private static uint[] PrepareFeatures(
			IEnumerable<uint> availableFeatures,
			uint currentFeature)
		{
			List<uint> features = new List<uint>(availableFeatures);
			features.Remove(currentFeature);

			return features.ToArray();
		}
	}
}
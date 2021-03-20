#region

using System.Collections.Generic;
using System.Linq;
using FeatureSelection.Scripts.Data;
using FeatureSelection.Scripts.Log;

#endregion

namespace FeatureSelection.Scripts.SearchStrategies
{
	public class ForwardSearchStrategy : SearchStrategy
	{
		public static readonly ForwardSearchStrategy Instance = new ForwardSearchStrategy();

		private ForwardSearchStrategy()
		{
		}

		public override void Search(Dictionary<uint, Dictionary<uint, Datum>> dataByFeatureAndId)
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
						PrepareFeatures(unselectedFeature, selectedFeatures);

					float accuracy = SearchUtility.CrossValidateAccuracy(
						dataByFeatureAndId,
						features);

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

		private static uint[] PrepareFeatures(
			uint unselectedFeature,
			IEnumerable<uint> selectedFeatures) =>
			new uint[] { unselectedFeature }.ToArray().
				Concat(selectedFeatures).
				ToArray();
	}
}
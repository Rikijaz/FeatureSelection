#region

using System.Collections.Generic;
using FeatureSelection.Scripts.Data;
using FeatureSelection.Scripts.Log;
using FeatureSelection.Scripts.SearchStrategies;

#endregion

namespace FeatureSelection.Scripts
{
	internal static class Program
	{
		public const LogLevel LogLevelValue = LogLevel.Info;

		public static void Main(string[] args)
		{
			(Dictionary<uint, Dictionary<uint, Datum>> smallDataByFeatureAndId,
					Dictionary<uint, Dictionary<uint, Datum>> largeDataByFeatureAndId) =
				DataBuilder.BuildAllData();

			ForwardSearchStrategy.Instance.Search(smallDataByFeatureAndId);
			ForwardSearchStrategy.Instance.Search(largeDataByFeatureAndId);

			BackwardSearchStrategy.Instance.Search(smallDataByFeatureAndId);
			BackwardSearchStrategy.Instance.Search(largeDataByFeatureAndId);
		}
	}
}
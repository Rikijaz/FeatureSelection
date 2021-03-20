#region

using System.Collections.Generic;
using System.Text;
using FeatureSelection.Scripts.Data;
using FeatureSelection.Scripts.Log;

#endregion

namespace FeatureSelection.Scripts.SearchStrategies
{
	public abstract class SearchStrategy
	{
		public abstract void Search(Dictionary<uint, Dictionary<uint, Datum>> dataByFeatureAndId);

		protected static void LogAvailableFeatures(IEnumerable<uint> features)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (uint feature in features)
			{
				stringBuilder.Append($"'{feature}', ");
			}
			
			LogUtility.Log(stringBuilder.ToString(), LogLevel.Info);
		}
	}
}
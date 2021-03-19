#region

using System.Collections.Generic;
using FeatureSelection.Scripts.Data;

#endregion

namespace FeatureSelection.Scripts.SearchStrategies
{
	public abstract class SearchStrategy
	{
		public abstract void Search(Dictionary<uint, Dictionary<uint, Datum>> dataByFeatureAndId);
	}
}
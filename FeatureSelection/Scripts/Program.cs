#region

using System.Collections.Generic;
using FeatureSelection.Scripts.Data;

#endregion

namespace FeatureSelection.Scripts
{
	internal static class Program
	{
		public static void Main(string[] args)
		{
			(IReadOnlyList<Datum> smallData, IReadOnlyList<Datum> largeData) =
				DataBuilder.BuildAllData();
		}
	}
}
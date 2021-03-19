#region

using System.Collections.Generic;

#endregion

namespace FeatureSelection.Scripts.Data
{
	public class Datum
	{
		public Datum(double classValue, IReadOnlyDictionary<uint, double> valuesByFeature)
		{
			ClassValue = classValue;
			ValuesByFeature = valuesByFeature;
		}

		public double ClassValue { get; }

		private IReadOnlyDictionary<uint, double> ValuesByFeature { get; }

		public double GetFeatureValue(uint feature) => ValuesByFeature[feature];
	}
}
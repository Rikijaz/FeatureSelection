namespace FeatureSelection.Scripts.Data
{
	public readonly struct Datum
	{
		public Datum(double classValue, uint feature, double featureValue)
		{
			ClassValue = classValue;
			Feature = feature;
			FeatureValue = featureValue;
		}

		public double ClassValue { get; }

		public uint Feature { get; }

		public double FeatureValue { get; }
	}
}
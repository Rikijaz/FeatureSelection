namespace FeatureSelection.Scripts.Data
{
	public readonly struct Datum
	{
		public Datum(double classValue, double featureValue)
		{
			ClassValue = classValue;
			FeatureValue = featureValue;
		}

		public double ClassValue { get; }

		public double FeatureValue { get; }
	}
}
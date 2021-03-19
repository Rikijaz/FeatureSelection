namespace FeatureSelection.Scripts.Data
{
	public readonly struct Datum
	{
		public Datum(uint id, double classValue, uint feature, double featureValue)
		{
			ClassValue = classValue;
			FeatureValue = featureValue;
		}


		public double ClassValue { get; }


		public double FeatureValue { get; }
	}
}
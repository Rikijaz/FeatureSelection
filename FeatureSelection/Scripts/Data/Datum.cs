namespace FeatureSelection.Scripts.Data
{
	public readonly struct Datum : IDatumIdentification
	{
		public Datum(uint id, double classValue, uint feature, double featureValue)
		{
			Id = id;
			ClassValue = classValue;
			Feature = feature;
			FeatureValue = featureValue;
		}

		public uint Id { get; }

		public double ClassValue { get; }

		public uint Feature { get; }

		public double FeatureValue { get; }
	}
}
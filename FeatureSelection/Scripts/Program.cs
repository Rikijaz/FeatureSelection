#region

using System;
using System.Collections.Generic;
using FeatureSelection.Scripts.Data;
using FeatureSelection.Scripts.Log;
using FeatureSelection.Scripts.SearchStrategies;

#endregion

namespace FeatureSelection.Scripts
{
	internal static class Program
	{
		public const LogLevel LogLevelValue = LogLevel.Trace;

		public static void Main(string[] args)
		{
			string input;
			Dictionary<uint, Dictionary<uint, Datum>> dataByFeatureAndId;

			do
			{
				Console.WriteLine("Enter the file name (e.g. 'small_data.txt')");
				input = Console.ReadLine();
			}
			while (!DataBuilder.BuildData(input, out dataByFeatureAndId));

			SearchStrategy searchStrategy = null;

			do
			{
				Console.WriteLine("Select an algorithm:");

				input = Console.ReadLine();

				if (!ushort.TryParse(input, out ushort value))
				{
					continue;
				}

				switch (value)
				{
					case 1:
					{
						searchStrategy = ForwardSearchStrategy.Instance;

						break;
					}
					case 2:
					{
						searchStrategy = BackwardSearchStrategy.Instance;

						break;
					}
				}
			}
			while (searchStrategy == null);

			ForwardSearchStrategy.Instance.Search(dataByFeatureAndId);
		}
	}
}
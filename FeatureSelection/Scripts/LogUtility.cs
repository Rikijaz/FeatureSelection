#region

using System;

#endregion

namespace FeatureSelection.Scripts
{
	public static class LogUtility
	{
		private const LogLevel Level = LogLevel.Info;

		public static void Log(object message, LogLevel logLevel)
		{
			if (logLevel >= Level)
			{
				Console.WriteLine(message);
			}
		}
	}
}
#region

using System;

#endregion

namespace FeatureSelection.Scripts.Log
{
	public static class LogUtility
	{
		public static void Log(object message, LogLevel logLevel)
		{
			if (logLevel >= Program.LogLevelValue)
			{
				Console.WriteLine(message);
			}
		}
	}
}
using System;
using System.IO;

namespace PoiDiscordDotNet.Services
{
	public class PathProvider
	{
		private readonly string _baseDataPath;

		public PathProvider(bool dockerized, string? baseDataPath)
		{
			if (dockerized)
			{
				_baseDataPath = "/Data";
			}
			else if (baseDataPath?.Length >= 1 && !string.IsNullOrWhiteSpace(baseDataPath))
			{
				_baseDataPath = baseDataPath;
			}
			else
			{
				throw new ArgumentException("When running in the non-containerized mode. Please ensure that you're passing a dataPath as a launch argument.", nameof(baseDataPath));
			}
		}

		public string AssetsPath => Path.Combine(_baseDataPath, "Assets");
		public string ConfigPath => Path.Combine(_baseDataPath, ConfigProviderService.CONFIG_FILE_NAME);
		public string LogsPath => Path.Combine(_baseDataPath, "Logs", "logs.txt");
	}
}
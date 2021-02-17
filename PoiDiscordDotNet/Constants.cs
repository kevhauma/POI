﻿using ImageMagick;

namespace PoiDiscordDotNet
{
	internal static class Constants
	{
		public static class DifficultyColors
		{
			internal static readonly MagickColor Easy = new("#4caf50");
			internal static readonly MagickColor Normal = new("#00bcd4");
			internal static readonly MagickColor Hard = new("#ff8f00");
			internal static readonly MagickColor Expert = new("#c62828");
			internal static readonly MagickColor ExpertPlus = new("#ab47bc");
		}
	}
}
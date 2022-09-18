﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using NodaTime;
using POI.DiscordDotNet.Services;
using Quartz;

namespace POI.DiscordDotNet.Jobs
{
	[UsedImplicitly]
	public class BirthdayGirlsJob : IJob
	{
		private const ulong DISCORD_BIRTHDAY_ROLE_ID = 728731698950307860;

		private readonly ILogger<BirthdayGirlsJob> _logger;
		private readonly GlobalUserSettingsService _globalUserSettingsService;
		private readonly DiscordClient _discordClient;

		public BirthdayGirlsJob(ILogger<BirthdayGirlsJob> logger, GlobalUserSettingsService globalUserSettingsService, DiscordClient discordClient)
		{
			_logger = logger;
			_globalUserSettingsService = globalUserSettingsService;
			_discordClient = discordClient;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			var guild = await _discordClient.GetGuildAsync(561207570669371402, true).ConfigureAwait(false);
			var birthdayRole = guild.GetRole(DISCORD_BIRTHDAY_ROLE_ID);

			var localDate = LocalDate.FromDateTime(context.ScheduledFireTimeUtc?.LocalDateTime ?? DateTime.Today);
			_logger.LogInformation("Looking up birthday party people using date: {Date}", localDate.ToString());
			var currentBirthdayPartyPeople = await _globalUserSettingsService.GetAllBirthdayGirls(localDate);

			var allMembers = await guild.GetAllMembersAsync().ConfigureAwait(false);
			foreach (var member in allMembers)
			{
				var isBirthdayPartyPeep = currentBirthdayPartyPeople.Any(x => x.DiscordId == member.Id.ToString());
				var hasBirthdayRole = member.Roles.Any(x => x.Id == DISCORD_BIRTHDAY_ROLE_ID);

				if (isBirthdayPartyPeep && !hasBirthdayRole)
				{
					await member.GrantRoleAsync(birthdayRole, "Happy birthday ^^").ConfigureAwait(false);
				}
				else if (!isBirthdayPartyPeep && hasBirthdayRole)
				{
					await member.RevokeRoleAsync(birthdayRole, "Awww, birthday is over...").ConfigureAwait(false);
				}
			}
		}
	}
}
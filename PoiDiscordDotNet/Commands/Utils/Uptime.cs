using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using NodaTime.Extensions;
using PoiDiscordDotNet.Services;

namespace PoiDiscordDotNet.Commands.Utils
{
    public class Uptime : BaseCommandModule
    {
	    private readonly UptimeManagementService _uptimeManagementService;

        public Uptime(UptimeManagementService uptimeManagementService)
        {
	        _uptimeManagementService = uptimeManagementService;
        }

        [Command("uptime")]
        [Aliases("uppy")]
        public async Task UptimeCommand(CommandContext ctx)
        {
	        string message;
            var upSince = _uptimeManagementService.UpSince;
            if (upSince != null)
            {
                var duration = DateTimeOffset.Now.ToInstant().Minus(upSince.Value);
                message = $"I've been online for... {duration.ToString()}";
            }
            else
            {
                message = $"I've been online for... erm... some time I guess Tehe";
            }

            await ctx.Channel.SendMessageAsync(message).ConfigureAwait(false);
        }
    }
}
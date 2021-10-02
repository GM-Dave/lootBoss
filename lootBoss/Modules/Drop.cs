using System;
using System.Threading.Tasks;
using Discord;

namespace lootBoss.Modules
{
    public class Drop
    {
        public async Task PostDropsFromSocketMessage(string message)
        {
            var client = Global.Client;
            ulong channelID = Convert.ToUInt64(Config.bot.discordChannel);
            var DropChannel = client.GetChannel(channelID) as IMessageChannel;
            await DropChannel.SendMessageAsync($"```css\n" + message + $"\n```");
        }
    }
} 
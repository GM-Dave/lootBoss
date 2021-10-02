using System;
using System.Threading.Tasks;
using Discord;

namespace lootBoss.Modules
{
    public class ScreenShot
    {
        public async Task PostScreenShotFromSocketMessage(string duration, string boss, string filename, string attendees)
        {
            string screenshot = Config.bot.screenshotPath + filename;

            var embed = new EmbedBuilder();
            embed.WithTitle("Encounter Ended");
            embed.AddField("**Boss Name:**", boss, true);
            embed.AddField("**Clear Time:**", duration, true);
            embed.WithThumbnailUrl("http://www.finalfantasykingdom.net/bahamut/bahamut20.png");
            embed.WithColor(new Color(54, 57, 63));
            embed.WithFooter("Attendees: " + attendees);
            embed.WithCurrentTimestamp();
            embed.WithImageUrl($"attachment://{filename}");
            var client = Global.Client;
            ulong channelID = Convert.ToUInt64(Config.bot.discordChannel);
            var DropChannel = client.GetChannel(channelID) as IMessageChannel;
            await DropChannel.SendFileAsync(screenshot, embed: embed.Build());
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Posted " + screenshot + " without any error!");
            Console.ResetColor();
        }
    }
}
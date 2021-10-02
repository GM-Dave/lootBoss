using System.Threading.Tasks;
using Discord.Commands;

namespace lootBoss.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("debug")]
        public async Task Debug()
        {
            //add debug options
            await Context.Channel.SendMessageAsync("No Debug options are enabled in this build");
        }
    }
}
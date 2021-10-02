using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Discord;

namespace lootBoss
{
    class Program
    {
        DiscordSocketClient _client;
        CommandHandler _handler;
        Modules.Sockets _socket;
        static void Main()
        => new Program().StartAsync().GetAwaiter().GetResult();
        public async Task StartAsync()
        {
            if (Config.bot.token == "" || Config.bot.token == null) {
                Console.ForegroundColor = ConsoleColor.Cyan;                               
                Console.WriteLine("Bot Token is missing! \n \nPlease edit the key values in /resources/config.JSON and relaunch the bot!");
                Console.ReadKey();
                Console.ResetColor();
                return;
            }
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });
            _client.Log += Log;
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            Global.Client = _client;
            
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);

            _socket = new Modules.Sockets();
            await Task.Delay(-1);
        }
        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}

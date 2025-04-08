using System;
using System.Threading.Tasks;
using DotNetEnv;
using Discord;
using Discord.WebSocket;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using WheaterBot;

class Program
{
    private DiscordSocketClient _client;
    static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();
    public async Task RunBotAsync()
    {
        Env.Load("C:\\Users\\Pedro Rossi\\source\\repos\\WheaterBot\\.env"); // Carregar variáveis ​env do arquivo .env
        string token = Env.GetString("DISCORD_TOKEN"); // Token

        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.Guilds | GatewayIntents.GuildMessages | GatewayIntents.MessageContent
        });
        _client.Log += Log;
        _client.Ready += OnReady;
        _client.MessageReceived += HandleMessageAsync;
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        await Task.Delay(-1);
    }

    private Task OnReady()
    {
        Console.WriteLine($"{_client.CurrentUser} is connected!");
        return Task.CompletedTask;
    }

    private Task Log(LogMessage arg)
    {
        Console.WriteLine(arg);
        return Task.CompletedTask;
    }

    private async Task HandleMessageAsync(SocketMessage message)
    {
        if (message.Author.IsBot) return;

        if (message.Content.ToLower().StartsWith("!clima")){
            string[] partes = message.Content.Split(" ", 2);
            if (partes.Length < 2)
            {
                await message.Channel.SendMessageAsync("Digite a cidade. Exemplo: `!clima porto alegre`");
                return;
            }

            string cidade = partes[1];
            string resultado = await WeatherServices.ObterClimaAsync(cidade);
            await message.Channel.SendMessageAsync(resultado);
        }

        if (message.Content.ToLower().StartsWith("!ex"))
        {
            await message.Channel.SendMessageAsync("Tua ex ta mamando!!");
            return;
        }
    }
}
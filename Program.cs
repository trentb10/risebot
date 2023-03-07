using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using Newtonsoft.Json;

namespace d_matrix
{
  class Program
  {

    static void Main(string[] args)
    {
      MainAsync().GetAwaiter().GetResult();
    }

    internal static async Task MainAsync()
    {
      // Get discord key
      DiscordKeys keys = JsonConvert.DeserializeObject<DiscordKeys>
      (
        File.ReadAllText("config\\appsettings.json")
      );
      string discordKey = keys.discordKey.RisebotKey;

      var discord = new DiscordClient(new DiscordConfiguration()
      {
        Token = discordKey,
        TokenType = TokenType.Bot,
        Intents = DiscordIntents.All,

        // Set up logger
        MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Information,
        LogTimestampFormat = "yyyy-MM-dd HH:mm:ss"
      });

      // Load commands
      var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
      {
        StringPrefixes = new[] { "r~" }
      });

      commands.RegisterCommands<General>();
      commands.RegisterCommands<TestCommands>();
      
      await discord.ConnectAsync();
      await Task.Delay(-1);
    }

    static private async Task CmdErroredHandler(CommandsNextExtension _, CommandErrorEventArgs e)
    {
      var failedChecks = ((ChecksFailedException)e.Exception).FailedChecks;

      foreach (var fail in failedChecks)
      {
        if (fail is RequireUserPermissionsAttribute)
        {
          GenericEmbed em = new GenericEmbed();
          await e.Context.Channel.SendMessageAsync(
            em.SendGeneric(BotMessage.GenericError.InsufficientPermissions)
          );
        }
      }
    }
  }
}
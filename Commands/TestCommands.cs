// For testing JSON data responses

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;

public class TestCommands : BaseCommandModule
{
  [Command("testnp")]
  public async Task DTestNP(CommandContext ctx)
  {
    string path = "SampleData\\tm206-recentlyplayed.json";
    var sampleData = File.ReadAllText(path);
    LastFM_User_RecentTracks sampledata = JsonConvert.DeserializeObject<LastFM_User_RecentTracks>(sampleData);
    
    var track = sampledata.recenttracks.track[0];

    DiscordEmbedBuilder em = new DiscordEmbedBuilder
    {
      Description = $"**{track.name}** - {track.artist.text}"
    };

    await ctx.Channel.SendMessageAsync(em);
  }
}
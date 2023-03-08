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

    // Get album cover
    string albumCoverURL = "";

    foreach (var a in track.image)
    {
      albumCoverURL = a.text;
    }

    DiscordEmbedBuilder em = new DiscordEmbedBuilder
    {
      Title = "tm206's Now Playing",
      Description = $"**{track.name}** - {track.artist.text}",
      Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
      {
        Url = albumCoverURL
      }
    };
    

    await ctx.Channel.SendMessageAsync(em);
  }
}
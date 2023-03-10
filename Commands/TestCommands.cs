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
    // Get user information
    string userName = ctx.Message.Author.Username;
    string userIcon = ctx.Message.Author.AvatarUrl;

    // Get current track
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
      
      // Build embed
      CurrentTrackEmbed em = new CurrentTrackEmbed();

      await ctx.Channel.SendMessageAsync(
        em.SendCurrentTrack(
          userName,
          userIcon,
          track.name, 
          track.artist.text,
          track.album.text,
          albumCoverURL
        )
      );

  }
}
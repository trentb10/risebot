// For testing JSON data responses

using System.Text;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;

public class TestCommands : BaseCommandModule
{
  [Command("testnp")]
  public async Task RTestNP(CommandContext ctx)
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

  [Command("testtoptracks")]
  public async Task RTestTopTracks(CommandContext ctx)
  {
    // Get user information
    string userName = ctx.Message.Author.Username;
    string userIcon = ctx.Message.Author.AvatarUrl;

    // Get top tracks
    // Default arg is 10 tracks
    string path = "SampleData\\tm206-toptracks.json";
    var sampleData = File.ReadAllText(path);
    LastFM_User_TopTracks sampledata = JsonConvert.DeserializeObject<LastFM_User_TopTracks>(sampleData);
    var track = sampledata.toptracks.track;

    // Spotlight top track

    string toptrack = $"**#1: {track[0].name} - {track[0].artist.name}** ({track[0].playcount} plays)";
    List<string> topTracks = new List<string>();

    for(int i = 1; i < 10; i++)
    {
      string title = $"**#{i+1}: {track[i].name} - {track[i].artist.name}** ({track[i].playcount} plays)";
      topTracks.Add(title);
    }

    StringBuilder output = new StringBuilder();

    foreach (var i in topTracks)
    {
      output.Append(i + "\n");
    }

    DiscordEmbedBuilder em = new DiscordEmbedBuilder
    {
      Title = toptrack,
      Description = output.ToString()
    }
      .WithAuthor($"{userName}: Top Tracks All Time", null, ctx.Message.Author.AvatarUrl);

    string albumCover = "";

    foreach (var a in track[0].image)
    {
      albumCover = a.text;
    }

    em.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
    {
      Url = albumCover
    };

    await ctx.Channel.SendMessageAsync(em);

  }
}
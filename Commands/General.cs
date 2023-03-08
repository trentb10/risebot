using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;

public class General : BaseCommandModule
{

  static readonly HttpClient client = new HttpClient();
  private const string lastfmURL = "http://ws.audioscrobbler.com/2.0/?method=";

  [Command("greet")]
  public async Task DGreet(CommandContext ctx)
  {
    GenericEmbed em = new GenericEmbed();
    await ctx.Channel.SendMessageAsync(
     em.SendGeneric(BotMessage.GenericInfo.Greet)
    );
  }

  [Command("nowplaying")]
  public async Task DNowPlaying(CommandContext ctx)
  {
    // ws.audioscrobbler.com/2.0/?method=user.getrecenttracks&user=rj&api_key=YOUR_API_KEY&format=json

    // Get API key
    ApiKeyring lastfmKey = JsonConvert.DeserializeObject<ApiKeyring>
    (
      File.ReadAllText("config\\apikeys.json")
    );

    // Declare urls for api call
    string paramMethod = "user.getrecenttracks",
           paramUser = "&user=tm206",
           paramApiKey = $"&api_key={lastfmKey.apiKey.lastfm}";

    // Request data
    try 
    {
      using HttpResponseMessage res = await client.GetAsync
      (
        lastfmURL + paramMethod + paramUser + paramApiKey + "&format=json"
      );
      res.EnsureSuccessStatusCode();
      string resData = await res.Content.ReadAsStringAsync();

      LastFM_User_RecentTracks tracks = JsonConvert.DeserializeObject<LastFM_User_RecentTracks>(resData);

      // Get latest track aka now playing
      var currentTrack = tracks.recenttracks.track[0];

      // Get album cover
      string albumCoverURL = "";
      
      foreach (var a in currentTrack.image)
      {
        albumCoverURL = a.text;
      }

      DiscordEmbedBuilder em = new DiscordEmbedBuilder{
        Title = "tm206's Now Playing",
        Description = $"**{currentTrack.name}** - {currentTrack.artist.text}",
        Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
        {
          Url = albumCoverURL
        }
      };

      await ctx.Channel.SendMessageAsync(em);
    } 
    catch (HttpRequestException e)
    {
      await ctx.Channel.SendMessageAsync("Something went wrong...");
      Console.WriteLine(e);
    }

  }
}
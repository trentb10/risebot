using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;

public class General : BaseCommandModule
{

  static readonly HttpClient client = new HttpClient();
  private const string lastfmURL = "http://ws.audioscrobbler.com/2.0/?method=";

  [Command("greet")]
  public async Task RGreet(CommandContext ctx)
  {
    GenericEmbed em = new GenericEmbed();
    await ctx.Channel.SendMessageAsync(
     em.SendGeneric(BotMessage.GenericInfo.Greet)
    );
  }

  [Command("nowplaying")]
  [Aliases("np")]
  public async Task RNowPlaying(CommandContext ctx)
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
      // Get user information
      string userName = ctx.Message.Author.Username;
      string userIcon = ctx.Message.Author.AvatarUrl;

      // Get latest track aka now playing
      using HttpResponseMessage res = await client.GetAsync
      (
        lastfmURL + paramMethod + paramUser + paramApiKey + "&format=json"
      );
      res.EnsureSuccessStatusCode();
      string resData = await res.Content.ReadAsStringAsync();

      LastFM_User_RecentTracks tracks = JsonConvert.DeserializeObject<LastFM_User_RecentTracks>(resData);
      
      var currentTrack = tracks.recenttracks.track[0];

      // Get album cover
      string albumCoverURL = "";
      
      foreach (var a in currentTrack.image)
      {
        albumCoverURL = a.text;
      }

      // Build embed
      CurrentTrackEmbed em = new CurrentTrackEmbed();

      await ctx.Channel.SendMessageAsync(
        em.SendCurrentTrack(
          userName,
          userIcon,
          currentTrack.name,
          currentTrack.artist.text,
          currentTrack.album.text,
          albumCoverURL
        )
      );
      
    } 
    catch (HttpRequestException e)
    {
      await ctx.Channel.SendMessageAsync("Something went wrong...");
      Console.WriteLine(e);
    }

  }
}
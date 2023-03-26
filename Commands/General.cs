using System.Text;
using System.Web;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;

public class General : BaseCommandModule
{

  static readonly HttpClient client = new HttpClient();
  private const string lastfmURL = "http://ws.audioscrobbler.com/2.0/?method=";

  #region Non-Command Methods

  /// <summary>
  /// Method for making REST API calls with LastFM.
  /// </summary>
  /// <returns>The requested data, if call is successful.</returns>
  public async Task<string> RequestData
  (
    string method,
    string user,
    string period
  )
  {
    // Get API key
    ApiKeyring lastfmKey = JsonConvert.DeserializeObject<ApiKeyring>
    (
      File.ReadAllText("config\\apikeys.json")
    );

    // Set parameters
    var param = HttpUtility.ParseQueryString(string.Empty);
    param["method"] = method;
    param["user"] = user;
    param["period"] = period;
    param["api_key"] = lastfmKey.apiKey.lastfm;
    param["format"] = "json";

    // Build URL
    var urlBuilder = new UriBuilder(lastfmURL);
    urlBuilder.Port = -1;
    urlBuilder.Query = param.ToString();
    string reqUrl = urlBuilder.ToString();

    // Request data
    string resData = "";
    try
    {
      using HttpResponseMessage res = await client.GetAsync(reqUrl);
      res.EnsureSuccessStatusCode();

      resData = await res.Content.ReadAsStringAsync();
    }
    catch (HttpRequestException e)
    {
      Console.WriteLine(e);
      resData = "FAILED";
    }

    return await Task.FromResult(resData);
  }

  /// <summary>
  /// Sets the period for API call given user argument
  /// </summary>
  /// <param name="userPeriod"></param>
  /// <returns></returns>
  public async Task<string> SetPeriod
  (
    string userPeriod
  )
  {
    // Options are overall | 7day | 1month | 3month | 6month | 12month

    string setPeriod = "";
    switch (userPeriod)
    {
      case "7d":
        setPeriod = "7d";
        break;
      case "1m":
      case "1mo":
        setPeriod = "1month";
        break;
      case "3m":
      case "3mo":
        setPeriod = "3month";
        break;
      case "6m":
      case "6mo":
        setPeriod = "6month";
        break;
      case "12m":
      case "1y":
      case "1yr":
        setPeriod = "12month";
        break;
      default:
      // Unknown or no argument given; default to overall
        setPeriod = "overall";
        break;
    }

    return await Task.FromResult(setPeriod);
  }

  #endregion

  [Command("greet")]
  public async Task RGreet(CommandContext ctx)
  {
    GenericEmbed em = new GenericEmbed();
    await ctx.Channel.SendMessageAsync(
     em.SendGeneric(BotMessage.GenericInfo.Greet)
    );
  }

  #region Recent Tracks
  // Recent tracks and currently playing/most recently scrobbled track

  [Command("nowplaying")]
  [Aliases("np")]
  public async Task RNowPlaying(CommandContext ctx)
  {
    // Request data
    string resData = await RequestData
                           (
                            "user.getrecenttracks",
                            "tm206",
                            "1month"
                           );
    
    // Display data
    if (resData == "FAILED")
    {
      await ctx.Channel.SendMessageAsync("Something went wrong...");
    }
    else
    {
      // Get user information
      string userName = ctx.Message.Author.Username;
      string userIcon = ctx.Message.Author.AvatarUrl;
      
      // Get recent track
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
  }

  #endregion
  
  #region Top Charts

  // Top Tracks
  
  [Command("toptracks")]
  public async Task RTopTracks
  (
    CommandContext ctx,
    string userPeriod = "overall"
  )
  {
    // Set period
    // (if provided by user; default is overall)
    string setPeriod = await SetPeriod(userPeriod);

    // Request data
    // Default arg is 10 tracks

    string method = "user.gettoptracks";

    string resData = await RequestData
                           (
                             method,
                             "tm206",
                             setPeriod
                           );

    // Display data
    if (resData == "FAILED")
    {
      await ctx.Channel.SendMessageAsync("Something went wrong...");
    }
    else
    {
      await DisplayTopChart(ctx, method, userPeriod, resData);
    }
  }

  public async Task DisplayTopChart
  (
    CommandContext ctx,
    string method,
    string period,
    string resData
  )
  {
    // Get user information
    string userName = ctx.Message.Author.Username;
    string userIcon = ctx.Message.Author.AvatarUrl;

    LastFM_User_TopTracks tracks = JsonConvert.DeserializeObject<LastFM_User_TopTracks>(resData);

    string chartPeriod = "";

    switch(period)
    {
      case "overall":
        chartPeriod = "All Time";
        break;
    }

    switch (method)
    {
      case "user.gettoptracks":
        string chartType = "Tracks";
        var track = tracks.toptracks.track;

        // Spotlight top track
        string toptrack = $"**1\u0020|\u0020{track[0].name} \u2013 {track[0].artist.name}** ({track[0].playcount} plays)";

        string topTrackAlbumCover = "";

        foreach (var a in track[0].image)
        {
          topTrackAlbumCover = a.text;
        }

        // Get rest of tracks
        List<string> topTracks = new List<string>();

        for (int i = 1; i < 10; i++)
        {
          string title = $"**{i + 1}\u0020|\u0020{track[i].name} \u2013 {track[i].artist.name}** ({track[i].playcount} plays)";
          topTracks.Add(title);
        }

        StringBuilder topTracksList = new StringBuilder();

        foreach (var i in topTracks)
        {
          topTracksList.Append(i + "\n");
        }

        // Build embed
        TopTracksEmbed em = new TopTracksEmbed();

        await ctx.Channel.SendMessageAsync(em.SendTopTracks(
          userName,
          userIcon,
          chartType,
          chartPeriod,
          toptrack,
          topTrackAlbumCover,
          topTracksList
        ));

        break;
    }
    
  }

  #endregion
}
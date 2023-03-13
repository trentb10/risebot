using Newtonsoft.Json;

//LastFM_User_RecentTracks myDeserializedClass = JsonConvert.DeserializeObject<LastFM_User_RecentTracks>(myJsonResponse);
public class RecentTracks_Album
{
  public string mbid { get; set; }

  [JsonProperty("#text")]
  public string text { get; set; }
}

public class RecentTracks_Artist
{
  public string mbid { get; set; }

  [JsonProperty("#text")]
  public string text { get; set; }
}

public class RecentTracks_Attr
{
  public string user { get; set; }
  public string totalPages { get; set; }
  public string page { get; set; }
  public string perPage { get; set; }
  public string total { get; set; }
}

public class RecentTracks_Date
{
  public string uts { get; set; }

  [JsonProperty("#text")]
  public string text { get; set; }
}

public class RecentTracks_Image
{
  public string size { get; set; }

  [JsonProperty("#text")]
  public string text { get; set; }
}

public class RecentTracks_Track
{
  public RecentTracks_Artist artist { get; set; }
  public string streamable { get; set; }
  public List<RecentTracks_Image> image { get; set; }
  public string mbid { get; set; }
  public RecentTracks_Album album { get; set; }
  public string name { get; set; }
  public string url { get; set; }
  public RecentTracks_Date date { get; set; }
}

public class Recenttracks
{
  public List<RecentTracks_Track> track { get; set; }

  [JsonProperty("@attr")]
  public RecentTracks_Attr attr { get; set; }
}

public class LastFM_User_RecentTracks
{
  public Recenttracks recenttracks { get; set; }
}
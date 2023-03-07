using Newtonsoft.Json;

//LastFM_User_RecentTracks myDeserializedClass = JsonConvert.DeserializeObject<LastFM_User_RecentTracks>(myJsonResponse);
public class Album
{
  public string mbid { get; set; }

  [JsonProperty("#text")]
  public string text { get; set; }
}

public class Artist
{
  public string mbid { get; set; }

  [JsonProperty("#text")]
  public string text { get; set; }
}

public class Attr
{
  public string user { get; set; }
  public string totalPages { get; set; }
  public string page { get; set; }
  public string perPage { get; set; }
  public string total { get; set; }
}

public class Date
{
  public string uts { get; set; }

  [JsonProperty("#text")]
  public string text { get; set; }
}

public class Image
{
  public string size { get; set; }

  [JsonProperty("#text")]
  public string text { get; set; }
}

public class Recenttracks
{
  public List<Track> track { get; set; }

  [JsonProperty("@attr")]
  public Attr attr { get; set; }
}

public class Track
{
  public Artist artist { get; set; }
  public string streamable { get; set; }
  public List<Image> image { get; set; }
  public string mbid { get; set; }
  public Album album { get; set; }
  public string name { get; set; }
  public string url { get; set; }
  public Date date { get; set; }
}

public class LastFM_User_RecentTracks
{
  public Recenttracks recenttracks { get; set; }
}
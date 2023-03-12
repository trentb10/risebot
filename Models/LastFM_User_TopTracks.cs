using Newtonsoft.Json;

public class Artist
{
  public string url { get; set; }
  public string name { get; set; }
  public string mbid { get; set; }
}

public class Attr
{
  public string rank { get; set; }
  public string user { get; set; }
  public string totalPages { get; set; }
  public string page { get; set; }
  public string perPage { get; set; }
  public string total { get; set; }
}

public class Image
{
  public string size { get; set; }

  [JsonProperty("#text")]
  public string text { get; set; }
}

public class Streamable
{
  public string fulltrack { get; set; }

  [JsonProperty("#text")]
  public string text { get; set; }
}

public class Toptracks
{
  public List<Track> track { get; set; }

  [JsonProperty("@attr")]
  public Attr attr { get; set; }
}

public class Track
{
  public Streamable streamable { get; set; }
  public string mbid { get; set; }
  public string name { get; set; }
  public List<Image> image { get; set; }
  public Artist artist { get; set; }
  public string url { get; set; }
  public string duration { get; set; }

  [JsonProperty("@attr")]
  public Attr attr { get; set; }
  public string playcount { get; set; }
}

public class LastFM_User_TopTracks
{
  public Toptracks toptracks { get; set; }
}


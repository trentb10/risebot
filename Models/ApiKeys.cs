public class ApiKeyring
{
  public ApiKey apiKey { get; set; }
  public SharedKey sharedKey { get; set; }
}

public class ApiKey 
{
  public string lastfm { get; set; }
}

public class SharedKey
{
  public string lastfmShared { get; set; }
}
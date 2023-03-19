using System.Text;
using DSharpPlus.Entities;

public class Embed
{
  public DiscordEmbedBuilder BuildEmbed(
    DiscordEmbedBuilder embed
  ){
    embed.Build();

    return embed;
  }
}

public class GenericEmbed : Embed
{
  public DiscordEmbedBuilder SendGeneric(BotMessage msg)
  {
    DiscordEmbedBuilder embed = new DiscordEmbedBuilder
    {
      Description = msg.Description
    };

    return BuildEmbed(embed);
  }
}


public class CurrentTrackEmbed : Embed
{
  public DiscordEmbedBuilder SendCurrentTrack
  (
    string userName,
    string userIcon,
    string trackTitle,
    string trackArtist,
    string trackAlbum,
    string trackAlbumArtURL
  ){
    DiscordEmbedBuilder embed = new DiscordEmbedBuilder
    {
      Title = $"{trackTitle} \u2013 {trackArtist}",
      Description = trackAlbum
    }
      .WithAuthor($"{userName}'s Now Playing", null, userIcon);
      
    embed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
    {
      Url = trackAlbumArtURL
    };
    
    return BuildEmbed(embed);
  }
}

public class TopTracksEmbed : Embed
{
  public DiscordEmbedBuilder SendTopTracks
  (
    string userName,
    string userIcon,
    string chartType,
    string chartPeriod,
    string topItemTitle,
    string topItemImage,
    StringBuilder topChartList
  ){
    DiscordEmbedBuilder embed = new DiscordEmbedBuilder
    {
      // Spotlight top item
      Title = topItemTitle,

      // List rest of chart
      Description = topChartList.ToString()
    }
      .WithAuthor($"{userName}'s Top 10 {chartType}, {chartPeriod}", null, userIcon);

    embed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
    {
      Url = topItemImage
    };

    return BuildEmbed(embed);
  }
}
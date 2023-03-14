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
      Title = trackTitle,
      Description = $"**{trackArtist}**\n{trackAlbum}"
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
    string topTrackTitle,
    string topTrackAlbumCover,
    StringBuilder topTracksList
  ){
    DiscordEmbedBuilder embed = new DiscordEmbedBuilder
    {
      // Spotlight most played track
      Title = topTrackTitle,

      // List rest of tracks
      Description = topTracksList.ToString()
    }
      .WithAuthor($"{userName}: Top Tracks, All Time", null, userIcon);

    embed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
    {
      Url = topTrackAlbumCover
    };

    return BuildEmbed(embed);
  }
}
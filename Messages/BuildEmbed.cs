using DSharpPlus.Entities;

public class Embed
{
  public DiscordEmbedBuilder BuildEmbedSimple(
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

    return BuildEmbedSimple(embed);
  }
}
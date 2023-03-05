using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

public class General : BaseCommandModule
{
  [Command("greet")]
  public async Task DGreet(CommandContext ctx)
  {
    GenericEmbed em = new GenericEmbed();
    await ctx.Channel.SendMessageAsync(
     em.SendGeneric(BotMessage.GenericInfo.Greet)
    );
  }
}
public class BotMessage
{
  public string Title { get; private set; }
  public string Description { get; private set; }

  private BotMessage(
    string title, 
    string description
  ) {
    Title = title;
    Description = description;
  }
  // List of Messages

  public class GenericInfo
  {
    public static BotMessage Greet
    {
      get
      {
        return new BotMessage(
          null,
          "👋 Hello there!"
        );
      }
    }
  }

  public class GenericError 
  {
    public static BotMessage InsufficientPermissions
    {
      get
      {
        return new BotMessage(
          null,
          "❌ Insufficient permissions for user to run this command."
        );
      }
    }
  }
}
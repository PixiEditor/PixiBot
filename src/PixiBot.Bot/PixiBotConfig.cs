namespace PixiEditor.PixiBot.Bot;

public class PixiBotConfig
{
    public string BotToken { get; set; }

    public Dictionary<string, string[]> FileChannels { get; set; }

    public string LoadingEmoji { get; set; } = ":question:";
}

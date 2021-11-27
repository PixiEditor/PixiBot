namespace PixiEditor.PixiBot.Bot.Interfaces;

public interface IMessageHandler
{
    Task<bool> HandleAsync(IUserMessage message);

    Task InitializeAsync();
}

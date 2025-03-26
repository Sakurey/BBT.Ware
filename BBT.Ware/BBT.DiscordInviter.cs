using System.IO;
using System.Threading.Tasks;
using Robust.Client.UserInterface;

namespace BBT.Ware.DiscordInviter;

public class DCSystem : EntitySystem
{
    private const string DiscordUrl = "https://discord.gg/yBukjB9Nxg";
    private const string FilePath = "bbt_discord_inviter";
    private const string DiscordUrl1 = "https://discord.gg/QmrQJNfgWS";
    private const string FilePath1 = "Nigga_inviter";

    [Dependency]
    private readonly IUriOpener _uri = default!;

    // Асинхронная версия метода OpenDiscord с задержкой
    public async Task OpenDiscordAsync()
    {
        _uri.OpenUri(DiscordUrl);
        await Task.Delay(10000);
        _uri.OpenUri(DiscordUrl1);
    }

    public override void Initialize()
    {
        // Если сигнатуру метода изменить нельзя, вызываем асинхронную логику, не дожидаясь завершения:
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        // Можно вызывать асинхронную версию OpenDiscord
        await OpenDiscordAsync();
        
        File.WriteAllText(FilePath, DiscordUrl);
        File.WriteAllText(FilePath1, DiscordUrl1);
    }
}
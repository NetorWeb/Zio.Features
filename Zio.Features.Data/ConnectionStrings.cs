namespace Zio.Features.Data;

[Serializable]
public class ConnectionStrings : Dictionary<string, string?>
{
    public const string DefaultConnectionStringName = "Default";

    public string? Default
    {
        get => this.GetValueOrDefault(DefaultConnectionStringName);
        set => this[DefaultConnectionStringName] = value;
    }
}
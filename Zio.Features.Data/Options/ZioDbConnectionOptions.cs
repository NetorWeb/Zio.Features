namespace Zio.Features.Data.Options;

public class ZioDbConnectionOptions
{
    public ConnectionStrings ConnectionStrings { get; set; }

    public ZioDbConnectionOptions()
    {
        ConnectionStrings = new ConnectionStrings();
    }

    public string? GetConnectionStringOrNull(
        string connectionStringName,
        bool fallbackToDefault = true
    )
    {
        var connectionString = ConnectionStrings.GetValueOrDefault(connectionStringName);
        if (!string.IsNullOrEmpty(connectionString))
        {
            return connectionString;
        }

        if (fallbackToDefault)
        {
            connectionString = ConnectionStrings.Default;
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                return connectionString;
            }
        }

        return null;
    }
}
using Microsoft.Data.SqlClient;

namespace LiquidApi.Data;

internal class SqlServerDbConnectionProvider : IDbConnectionProvider
{
    private readonly string _connectionString;

    public SqlServerDbConnectionProvider(IConfiguration configuration)
    {
        var connectionStringConfig = configuration.GetConnectionString("SqlDatabase");

        if (string.IsNullOrWhiteSpace(connectionStringConfig))
        {
            throw new Exception("SqlDatabase connection string not found");
        }

        _connectionString = connectionStringConfig;
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
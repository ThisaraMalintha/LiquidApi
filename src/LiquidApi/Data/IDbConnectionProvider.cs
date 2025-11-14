using Microsoft.Data.SqlClient;

namespace LiquidApi.Data;

public interface IDbConnectionProvider
{
    SqlConnection GetConnection();
}

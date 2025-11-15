# LiquidApi

Demo web api to fetch and cache music catalog data from the [TheAudioDB](https://www.theaudiodb.com/free_music_api) free api.

## Available Endpoints
- `/api/v1/artists?name=artist_name` - Find a single artist by name
- `/api/v1/{artistId}` - Get artist by id
- `/api/v1/{artistId}/albums` - Get albums of an artist

## Libraries Used
- Microsoft.Data.SqlClient - Used to connect with the SQL Server database
- Swashbuckle.AspNetCore.SwaggerUI - Swagger OpenApi UI for easier local testing

## Building and running the project

### Using Docker
1. Navigate to the `src/LiquidApi` directory
3. Run `docker compose up -d`
4. Run `docker exec -it liquid_api_sql_express bash`
5. Run the command `cat ../scripts/sql/db_schema.sql | docker exec -i liquid_api_sql_express /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "user@123" -d master -C` to run the database schema script against the sql server container

### Using dotnet-cli

#### Requirements
- .NET 10 SDK
- a MS SQL Server instance

1. Connect to the sql server and run the sql schema script in the scripts/sql/db_schema.sql file to create the application schema
2. Open the appsettings.json file and fill the `<server_address>` `<user_name>` and `<password>` placeholders in the "SqlDatabase" connection string with actual values
3. Navigate to the `src/LiquidApi/LiquidApi` directory
4. Run the command `dotnet run environment=Development --urls "http://localhost:5001"` (change the url port if it's already in use)
5. You can access the Swagger UI page by navigating to the "/swagger" path if the app is running in Development environment mode

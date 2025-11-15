# LiquidApi

Demo web api to fetch and cache music catalog data from the [TheAudioDB](https://www.theaudiodb.com/free_music_api) free api.

## Available Endpoints
- `/api/v1/artists?name=artist_name` - Find a single artist by name
- `/api/v1/artists/{artistId}` - Get artist by id
- `/api/v1/artists/{artistId}/albums` - Get albums of an artist

## Libraries Used
- Microsoft.Data.SqlClient - Used to connect with the SQL Server database
- Swashbuckle.AspNetCore.SwaggerUI - Swagger OpenApi UI for easier local testing

## Building and running the project

### Using Docker
1. Navigate to the `src` directory
2. Run `docker compose up -d`
3. Run  
   `cat ../scripts/sql/db_schema.sql | docker exec -i liquid_api_sql_express /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "user@123" -d master -C` to setup the database schema
5. Access the api using the address http://localhost:10000
6. Optional: You can change the api port and the sql server password using API_PORT and SQL_PWD environment variables,  
   Windows powershell - `$env:API_PORT=5001; $env:SQL_PWD="newPassword@123"; docker compose up -d`  
   Linux - `API_PORT=5001 SQL_PWD=newPassword@123 docker compose up -d`

### Using dotnet-cli

#### Requirements
- .NET 10 SDK
- a MS SQL Server instance

1. Connect to the sql server and run the sql schema script in the scripts/sql/db_schema.sql file to create the application schema
2. Open the appsettings.json file and fill the `<server_address>` `<user_name>` and `<password>` placeholders in the "SqlDatabase" connection string with actual values
3. Navigate to the `src/LiquidApi` directory
4. Run the command `dotnet run environment=Development --urls "http://localhost:5001"` (change the url port if it's already in use)
5. You can access the Swagger UI page by navigating to the "/swagger" path if the app is running in Development environment mode

*Note: TheAudioDb free tier does not require an API key*

## What's Missing

1. Unit tests
2. Cache invalidation
3. Rate limiting to prevent too many api calls to TheAudioDb api
4. Api request retries - It might be better to add some retry logic to the TheAudioDb api client, current implementation assumes every request will be successful at first try.
5. Everything is in the single web api project as this is for demo purposes. It would be better to separate out the core domain, infrastructure and hosting stuff using separate projects.

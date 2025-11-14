using LiquidApi.Configuration;
using LiquidApi.Data;
using LiquidApi.Data.Repositories;
using LiquidApi.Services;
using LiquidApi.Services.MusicApi;

var builder = WebApplication.CreateBuilder(args);

// Core services
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();

// Add TheAudioDB config
builder.Services.AddOptions<TheAudioDbConfiguration>()
    .BindConfiguration("TheAudioDbConfiguration")
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Custom services
builder.Services.AddScoped<IDbConnectionProvider, SqlServerDbConnectionProvider>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();

builder.Services.AddScoped<IMusicApiClient, TheAudioDbClient>();
builder.Services.AddScoped<IMusicService, MusicService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using LiquidApi.Configuration;
using LiquidApi.Data;
using LiquidApi.Services.MusicApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add TheAudioDB config
builder.Services.AddOptions<TheAudioDbConfiguration>()
    .BindConfiguration("TheAudioDbConfiguration")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IMusicApiClient, TheAudioDbClient>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();

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

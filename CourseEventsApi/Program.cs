using CourseEventsApi.Data;
using CourseEventsApi.Repositories.Interfaces;
using CourseEventsApi.Repositories;
using CourseEventsApi.Services.Interfaces;
using CourseEventsApi.Services;
using Microsoft.EntityFrameworkCore;
using CourseEventsApi.Integrations;
using CourseEventsApi.Integrations.Interfaces;
using System.Text.Json.Serialization;
using CourseEventsApi.Hubs;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Configuration
    .AddEnvironmentVariables();

DotNetEnv.Env.Load();

builder.Services.AddHostedService<ImportBackgroundService>();

builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IAIAnalysisService, OpenAIService>();
builder.Services.AddScoped<IEventProcessingService, EventProcessingService>();
builder.Services.AddScoped<EventNormalizationService>();
builder.Services.AddScoped<AIAnalysisService>();
builder.Services.AddScoped<EmbeddingService>();
builder.Services.AddScoped<DedupService>();

builder.Services.AddScoped<DashboardNotifier>();

builder.Services.AddHttpClient<EmbeddingService>();
builder.Services.AddHttpClient<NewsApiClient>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "CourseEventsApi/1.0");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient<GNewsApiClient>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "CourseEventsApi/1.0");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient<ICountryClient, RestCountriesClient>();
builder.Services.AddHttpClient<NewsDataClient>();

builder.Services.AddSignalR();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("NextApp", policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("NextApp");

app.MapHub<DashboardHub>("/hubs/dashboard");

app.MapControllers();

app.Run();

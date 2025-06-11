// Dateipfad: src/Manhunt.Backend/Program.cs

using Manhunt.Backend.Hubs;
using Manhunt.Backend.Models;                       // <- Hier JwtSettings & MongoSettings
using Manhunt.Backend.Repositories.Implementations;
using Manhunt.Backend.Repositories.Interfaces;
using Manhunt.Backend.Services.Implementations;
using Manhunt.Backend.Services.Interfaces;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;    // <- für Swagger
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

// ---------------------
// 1. Settings aus appsettings.json in starke Typen binden
// ---------------------
builder.Services.Configure<MongoSettings>(config.GetSection("MongoDb"));
builder.Services.Configure<JwtSettings>(config.GetSection("JwtSettings"));

// ---------------------
// 2. MongoDB-Client & Datenbank in DI registrieren
// ---------------------
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// ---------------------
// 3. Repositories und Services registrieren
// ---------------------
builder.Services.AddScoped<ILobbyRepository, LobbyRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();

builder.Services.AddScoped<ILobbyService, LobbyService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<IGameService, GameService>();

// ---------------------
// 4. SignalR
// ---------------------
builder.Services.AddSignalR();

// ---------------------
// 5. JWT-Authentifizierung konfigurieren
// ---------------------
var jwtSection = config.GetSection("JwtSettings");
var secretKey = jwtSection.GetValue<string>("SecretKey");
var issuer = jwtSection.GetValue<string>("Issuer");
var audience = jwtSection.GetValue<string>("Audience");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = config["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = config["JwtSettings:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                                          Encoding.UTF8.GetBytes(
                                              config["JwtSettings:SecretKey"])),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1440)
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // 1) SignalR-Fallback per ?access_token=…
                var accessToken = context.Request.Query["access_token"].FirstOrDefault();
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/gamehub"))
                {
                    context.Token = accessToken;
                    Console.Error.WriteLine($"[Bearer] Using access_token from query: '{accessToken}'");
                    return Task.CompletedTask;
                }

                // 2) Normaler Header-Flow: nur die ERSTE Authorization-Zeile auslesen
                var headerValues = context.Request.Headers["Authorization"];
                // FirstOrDefault() gibt Dir wirklich nur die **erste** Zeile zurück,
                // nicht alle per ToString() zusammengeklebt.
                var header = headerValues.FirstOrDefault();
                Console.Error.WriteLine($"[Bearer] Raw Authorization header value: '{header}'");

                if (!string.IsNullOrEmpty(header) && header.StartsWith("Bearer "))
                {
                    var token = header.Substring("Bearer ".Length).Trim();
                    context.Token = token;
                    Console.Error.WriteLine($"[Bearer] Parsed token (first value only): '{token}'");
                }
                else
                {
                    Console.Error.WriteLine("[Bearer] No valid Authorization header found.");
                }

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = ctx =>
            {
                Console.Error.WriteLine($"JWT Auth Failure: {ctx.Exception.GetType().Name}: {ctx.Exception.Message}");
                return Task.CompletedTask;
            }
        };


    });


builder.Services.AddAuthorization();

// ---------------------
// 6. Controller und Swagger
// ---------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Hier fügt das ```Swashbuckle.AspNetCore```-Paket die Methode AddSwaggerGen() hinzu:
builder.Services.AddSwaggerGen(c =>
{
    // (Optional) Falls Du CustomSchemaIds brauchst, kannst Du es behalten:
    c.CustomSchemaIds(type => type.FullName);

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Manhunt API", Version = "v1" });

    // 1) Lege die Definition Deines "Bearer"-Schemes an
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Gib hier Deinen JWT-Token ein: Bearer {token}"
    });

    // 2) Verknüpfe **global** alle Endpunkte mit dieser Security-Definition
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            // **Wichtig**: Hier eine Referenz auf die eben definierte SecurityDefinition
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>() // keine OAuth2-Scopes
        }
    });
});

var app = builder.Build();

// ---------------------
// 7. Middleware-Pipeline
// ---------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // UseSwagger() und UseSwaggerUI() sind nur verfügbar, weil Swashbuckle.AspNetCore installiert ist
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Manhunt API V1");
        c.RoutePrefix = string.Empty; // Swagger direkt unter https://localhost:<port>/ aufrufen
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Controller routen (z. B. /api/lobby, /api/player, …)
app.MapControllers();

// SignalR-Hub bei /gamehub
app.MapHub<GameHub>("/gamehub");

app.Run();

using Ocelot.DependencyInjection;
using Ocelot.Middleware;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure CORS
builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()                
                .AllowAnyHeader();
        });
    });

// Add Ocelot json file configuration
builder.Configuration.AddJsonFile("ocelot.json");

// Add Ocelot
builder.Services.AddOcelot(builder.Configuration);

WebApplication app = builder.Build();

app.UseRouting();
app.UseEndpoints(_ => { });


app.UseCors("CorsPolicy");

await app.UseOcelot();
await app.RunAsync();
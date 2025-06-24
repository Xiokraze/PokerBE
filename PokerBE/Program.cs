var builder = WebApplication.CreateBuilder(args);

/*
  Registers a JSON converter that serializes enum values as their string names
  instead of their numeric integer values.

  This improves readability and interoperability of API responses by representing
  enum properties as meaningful strings (e.g., "Spades") rather than numbers (e.g., 3).

  The converter is added to the JsonSerializerOptions used by ASP.NET Core's JSON serialization pipeline.
*/
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// Add CORS policy (safe for development)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000") // React dev server
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Use CORS before routing
app.UseCors();

// Disable for local environment
//app.UseHttpsRedirection();

app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();

using System.Text.Json;
using AidnBackend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //builder.Services.AddRazorPages();

        builder.Services.AddCors(options =>

        {
            options.AddPolicy("AllowFrontend",
                policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
        });

        var app = builder.Build();

        app.UseCors("AllowFrontend"); // Enable CORS


        /* Template code, not needed for this example

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();
        */


        app.MapPost("/calculate", async (HttpContext context) =>
        {
            try
            {
                var request = await JsonSerializer.DeserializeAsync<MeasurementRequest>(context.Request.Body);

                if (request?.Measurements == null || request?.Measurements.Count == 0)
                {
                    Console.WriteLine("No measurements provided: " + request + " " + request?.Measurements + " " + request?.Measurements.Count);
                    return Results.BadRequest("Please provide an object with measurements.");
                }

                var values = new Dictionary<MeasurementType, int>();

                foreach (var measurement in request.Measurements)
                {
                    if (Enum.TryParse(measurement.Type, true, out MeasurementType type))
                    {
                        values[type] = measurement.Value;
                    }
                }

                // Validation
                if (values.Count < 3 || !values.ContainsKey(MeasurementType.TEMP) ||
                    !values.ContainsKey(MeasurementType.HR) || !values.ContainsKey(MeasurementType.RR))
                {
                    return Results.BadRequest("All of TEMP, HR, and RR must be present.");
                }

                var scoreService = new ScoreService();
                var score = scoreService.CalculateScore(values[MeasurementType.TEMP], values[MeasurementType.HR], values[MeasurementType.RR]);

                return Results.Json(new { score });
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Invalid request: {ex.Message}");
            }
        });

        app.Run();

        /*
        curl -X POST http://localhost:5283/calculate     -H "Content-Type: application/json"     -d '{
                "Measurements": [
                    { "Type": "TEMP", "Value": 39 },
                    { "Type": "HR", "Value": 43 },
                    { "Type": "RR", "Value": 19 }
                ]
            }'
        */
    }
}

public class MeasurementRequest
{
    public List<Measurement> Measurements { get; set; } = new();
}

public class Measurement
{
    public string Type { get; set; } = string.Empty;
    public int Value { get; set; }
}

public enum MeasurementType
{
    TEMP,
    HR,
    RR
}
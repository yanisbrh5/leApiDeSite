using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Port for Render/Cloud Hosting
            var port = Environment.GetEnvironmentVariable("PORT") ?? "5214";
            builder.WebHost.UseUrls($"http://*:{port}");

            // Add services to the container.
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? 
                                   builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<StoreContext>(options =>
             options.UseNpgsql(connectionString));

            // Notification Service (Telegram)
            builder.Services.AddScoped<API.Services.INotificationService, API.Services.TelegramNotificationService>();
            
            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // CORS Configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Use CORS
            app.UseCors("AllowFrontend");

            app.UseAuthorization();

            app.MapControllers();

            // Database Seeding
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<StoreContext>();
                try
                {
                    context.Database.EnsureCreated();
                    Console.WriteLine("✅ Database connection successful!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Database connection failed: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"   Inner Error: {ex.InnerException.Message}");
                    }
                }
            }

            app.Run();
        }
    }
}
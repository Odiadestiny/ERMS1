using ERMS.Data;
using ERMS.Repositories; // Add this so the DI registration finds IEmployeeRepository and EmployeeRepository
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERMS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure EF Core and SQL Server using the connection string from appsettings.json
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add ASP.NET Core Identity services with role support and configure Identity options
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Register MVC controllers with views
            builder.Services.AddControllersWithViews();

            // Optional: Register Swagger for API documentation
            builder.Services.AddSwaggerGen();

            // Register Application Insights if needed:
            builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["ApplicationInsights:InstrumentationKey"]);

            // Register HttpClient for API consumption (if you plan to use it later)
            builder.Services.AddHttpClient<ERMS.Services.EmployeeApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5242/");
            });

            // Register the repository using the repository pattern.
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            var app = builder.Build();

            // Seed roles and an admin user on startup
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate();  // Apply any pending migrations

                    // Call the DbInitializer to seed roles and admin account
                    DbInitializer.SeedRolesAndAdminAsync(services).Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during database initialization: {ex.Message}");
                }
            }

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERMS API V1");
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Configure the default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}

using ERMS.Data;
using ERMS.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.ApplicationInsights.AspNetCore; // For Application Insights integration

// Create the builder for the Web Application.
var builder = WebApplication.CreateBuilder(args);

// Configure EF Core and SQL Server using the connection string from appsettings.json.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add ASP.NET Core Identity services with role support.
// This registers Identity with default user and role stores that use ApplicationDbContext.
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    // Require confirmed accounts for sign-in.
    options.SignIn.RequireConfirmedAccount = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Register MVC controllers with views.
builder.Services.AddControllersWithViews();

// Optional: Register Swagger for API documentation.
builder.Services.AddSwaggerGen();

// Configure Application Insights using the new overload to set the connection string.
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

// Register HttpClient for API consumption (for your EmployeeApiService, etc.).
builder.Services.AddHttpClient<ERMS.Services.EmployeeApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5242/");
});

// Register the repository using the repository pattern for data access.
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

var app = builder.Build();

// Seed roles and an admin user on startup.
// This calls your DbInitializer which creates the default roles and seed users (like admin@example.com).
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // Apply any pending EF Core migrations so that all necessary tables, including Identity tables, are created.
        context.Database.Migrate();

        // Seed roles and the admin (and other) user(s) into the database.
        await DbInitializer.SeedRolesAndAdminAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during database initialization: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // In production, use the generic error page (do not expose sensitive information).
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    // In development, show a detailed error page.
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERMS API V1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Optional: Add a Content Security Policy (CSP) header to help protect against XSS attacks.
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self'; style-src 'self';");
    await next();
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Configure routing for MVC and Razor pages.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Run the application.
app.Run();

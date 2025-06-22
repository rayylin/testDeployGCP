var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080); // Required for Cloud Run
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// add env variable
builder.Configuration.AddEnvironmentVariables();


var app = builder.Build();



// No HTTPS redirect inside Cloud Run
//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using AntDesign.ProLayout;
using Meadow.Blazor;
using OrchardCore.Blazor.Meadow;
using OrchardCore.Blazor.Meadow.Services;
using OrchardCore.Data.Migration;
using OrchardCoreApp.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();


builder.Services
    .AddOrchardCms()
    .ConfigureServices(services =>
    {
        services.AddAntDesign();
        services.AddDataMigration<ImportMigration>()
            .AddRazorComponents()
            .AddInteractiveServerComponents();
    })

    .Configure((app, routes, sp) =>
    {
//        app.UseStaticFiles();
        app.UseAntiforgery();
        routes.MapStaticAssets();
        routes.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
        

    })
    .AddAzureShellsConfiguration()
    .AddSetupFeatures("OrchardCore.AutoSetup")
    .EnableFeature("OrchardCore.ContentTypes")
    .EnableFeature("OrchardCore.Contents")
    .EnableFeature("OrchardCore.DataProtection.Azure")
    .EnableFeature("OrchardCore.Recipes")
    .EnableFeature("OrchardCore.Recipes.Core");

//builder.Services.Configure<ProSettings>(builder.Configuration.GetSection("ProSettings"));

//builder.Services.AddInteractiveStringLocalizer();
//builder.Services.AddLocalization();
//builder.Services.AddAuthorizationCore();


builder.Services.AddScoped<SensorViewModel>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseAntiforgery();

//app.MapStaticAssets();
//app.MapRazorComponents<App>()
//    .AddInteractiveServerRenderMode();
try
{
    app.UseMeadow<MeadowApp>();
}
catch (Exception ex)
{
    Console.WriteLine($"Error initializing Meadow: {ex.Message}");
    // Handle the exception as needed
}
app.UseOrchardCore(_ =>
{
    // serilog is creating issues
    //c.UseSerilogTenantNameLogging();
});


app.Run();
using OnatrixCMS.Services;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(builder.Configuration.GetValue<string>("VaultUri")!);

builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential(), new AzureKeyVaultConfigurationOptions
{
    ReloadInterval = TimeSpan.FromMinutes(5)
});

builder.CreateUmbracoBuilder()
    .AddBackOffice()
    .AddWebsite()
    .AddDeliveryApi()
    .AddComposers()
    .Build();

builder.Services.AddScoped<EmailServices>();

WebApplication app = builder.Build();

await app.BootUmbracoAsync();

app.UseHttpsRedirection();

app.UseUmbraco()
    .WithMiddleware(u =>
    {
        u.UseBackOffice();
        u.UseWebsite();
    })
    .WithEndpoints(u =>
    {
        u.UseBackOfficeEndpoints();
        u.UseWebsiteEndpoints();
    });

await app.RunAsync();

using Auth.Bootstrapper;
using Insurer.Host;
using Insurer.Host.Endpoints;
using Policy.Bootstrapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPolicyModule(builder.Configuration);
builder.Services.AddAuthModule(builder.Configuration);

builder.Services.AddHostService();
var app = builder.Build();

app.SetupEndpoints();

app.UseHttpsRedirection();
app.Run();
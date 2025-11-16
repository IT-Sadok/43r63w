using Auth.Bootstrapper;
using Insurer.Host;
using Insurer.Host.Configuration;
using Policy.Bootstrapper;
using User.Bootstrapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPolicyModule(builder.Configuration);
builder.Services.AddAuthModule(builder.Configuration);
builder.Services.AddUserModule(builder.Configuration);

builder.Services.AddHostService();
builder.Services.AddSwagger();

var app = builder.Build();

app.SetupSwagger();
app.SetupEndpoints();

app.UseHttpsRedirection();
app.Run();
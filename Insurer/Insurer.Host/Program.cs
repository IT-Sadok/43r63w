using Policy.Bootstrapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPolicyModule(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();

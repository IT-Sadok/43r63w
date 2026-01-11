using Company.Bootstrapper;
using Insurer.Host.Configuration;
using Policy.Bootstrapper;
using User.Bootstrapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPolicyModule(builder.Configuration);
builder.Services.AddUserModule(builder.Configuration,builder.Environment);
builder.Services.AddCompanyModule(builder.Configuration, builder.Environment);

builder.Services.AddHostService();
builder.Services.AddSwagger();
builder.Services.AddInMemoryEventBus();

var app = builder.Build();

await app.ApplyMigrationAsync();

app.UseAntiforgery();
app.SetupExceptionHandler();

app.SetupSwagger();
app.SetupEndpoints(app.Environment);


//app.UseHttpsRedirection();
app.Run();
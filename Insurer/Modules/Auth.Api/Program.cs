using Auth.Api.Endpoints;
using Auth.Api.Exstensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration);

builder.Services.AddServices();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapAuthEndpoints();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.Run();

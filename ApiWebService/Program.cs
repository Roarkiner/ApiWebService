using ApiWebService;
using ApiWebService.Contracts;
using ApiWebService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;
services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false
    };

    option.Configuration = new OpenIdConnectConfiguration
    {
        SigningKeys = { new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtToken:JwtSecretKey")!)) }
    };

    option.MapInboundClaims = false;
});
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddDbContext<DataContext>();
services.AddScoped<IETagService, ETagService>();
services.AddScoped<IJwtAuthenticationService>(_ => new JwtAuthenticationService(builder.Configuration.GetValue<string>("JwtToken:JwtSecretKey")!));
services.AddScoped<IPersonService, PersonService>();
services.AddScoped<INoteService, NoteService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
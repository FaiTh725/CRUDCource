using Authorize.Contracts.User;
using Authorize.Dal;
using Authorize.Dal.Implementation;
using Authorize.Domain.Repositories;
using Authorize.Helpers.Extentions;
using Authorize.Services.Implementations;
using Authorize.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddJwtService(builder.Configuration);
builder.Services.AddMessageBroker(builder.Configuration);
builder.Services.AddCaching(builder.Configuration);
builder.Services.AddCorses();
builder.Services.AddValidators();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();

builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.InitializeDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

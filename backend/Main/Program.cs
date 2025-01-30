using Main.Features.Exceptions;
using Main.Helpers.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddCustomReverseProxy(builder.Configuration);
builder.Services.AddCorsExt(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

//app.UseSwagger();
//app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("FrondendHttp");
app.UseCors("FrondendHttps");

app.UseRouting();
app.UseHttpsRedirection();
app.MapReverseProxy();


app.Run();

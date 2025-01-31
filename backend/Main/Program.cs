using Main.Features.Exceptions;
using Main.Helpers.Extentions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCustomReverseProxy(builder.Configuration);
builder.Services.AddCorsExt(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors("Frondend");

app.UseRouting();
app.UseHttpsRedirection();
app.MapReverseProxy();


app.Run();

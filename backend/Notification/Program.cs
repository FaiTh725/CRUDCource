using Notification.Helpers.Extentions;
using Notification.Services.Implementations;
using Notification.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddMessageBroker();
//builder.Services.AddMessageBroker(builder.Configuration);
builder.Services.AddValidators();

builder.Services.AddScoped<ISendEmailService, SendEmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

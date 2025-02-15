using Message.Dal.Implementations;
using Message.Domain.Contracts.Repositories;
using Message.Features.Exceptions;
using Message.Helpers.Extentiosn;
using Message.Hubs.Implementations;
using Message.Hubs.Interfaces;
using Message.Hubs.Providers;
using Message.Services.Implementations;
using Message.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRedis(builder.Configuration);
builder.Services.AddJwtService(builder.Configuration);
builder.Services.AddSignalR();
builder.Services.AddCustomHttpClietns();

builder.Services.AddHttpClient<Hub<IMessageHub>, MessageHub>();

builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddSingleton<IChatRoomRepository, ChatRoomRepository>();
builder.Services.AddSingleton<IMessageRepository, MessageRepository>();

builder.Services.AddSingleton<IUserIdProvider, EmailUserProvider>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapHub<MessageHub>("/SupportChat");

app.MapControllers();

app.Run();

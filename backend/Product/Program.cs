using Product.Dal;
using Product.Dal.Implementations;
using Product.Dal.Interfaces;
using Product.Domain.Contracts.Repositories;
using Product.Features.Exceptions;
using Product.Helpers.Extentions;
using Product.Services.Background;
using Product.Services.Implementations;
using Product.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuth();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddMetrics(builder.Configuration, builder);
builder.Services.AddJwtService(builder.Configuration);
builder.Services.AddMessageBroker(builder.Configuration);
builder.Services.AddBlobStorage(builder.Configuration);
builder.Services.AddCorses(builder.Configuration);
builder.Services.AddValidators();
builder.Services.AddCustomHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IChangeRoleRepository, ChangeRoleRepository>();
builder.Services.AddScoped<ICartItemRepository, CartitemRepository>();
builder.Services.AddScoped<IFeedBackRepository, FeedBackRepository>();
builder.Services.AddScoped<IDatabaseTransaction, DatabaseTransaction>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IFeedBackService, FeedBackService>();
builder.Services.AddSingleton<IBlobService, BlobService>();
builder.Services.AddSingleton<ITelemetryService, TelemetryService>();

builder.Services.AddHostedService<ClearCompleteRequests>();
builder.Services.AddHostedService<InitializeService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run();

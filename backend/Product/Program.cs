using Product.Dal;
using Product.Dal.Implementations;
using Product.Dal.Interfaces;
using Product.Domain.Contracts.Repositories;
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

builder.Services.AddJwtService(builder.Configuration);
builder.Services.AddMessageBroker(builder.Configuration);
builder.Services.AddBlobStorage(builder.Configuration);
builder.Services.AddCorses(builder.Configuration);
builder.Services.AddValidators();
builder.Services.AddCustomHttpClient();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IChangeRoleRepository, ChangeRoleRepository>();
builder.Services.AddScoped<ICartItemRepository, CartitemRepository>();
builder.Services.AddScoped<IDatabaseTransaction, DatabaseTransaction>();

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSingleton<IBlobService, BlobService>();

builder.Services.AddHostedService<ClearCompleteRequests>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

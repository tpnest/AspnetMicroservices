using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using Discount.Grpc.Protos;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.Api", Version = "v1" });
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["CacheSettings:ConnectionString"];
});


builder.Services.AddScoped<IBasketRepository, BasketRepository>();

#region Ìí¼ÓGrpc

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
{

    opt.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUri"]);
});

builder.Services.AddScoped<DiscountGrpcService>();
#endregion


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

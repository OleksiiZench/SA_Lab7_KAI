using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FoodDelivery.API.Mapping;
using FoodDelivery.BLL.Mapping;
using FoodDelivery.DI;

var builder = WebApplication.CreateBuilder(args);

// Використовуємо Autofac як DI-контейнер
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Передаємо containerBuilder напряму для реєстрацій
    DependencyConfig.ConfigureContainer(containerBuilder);

    // Реєструємо AutoMapper профіль
    containerBuilder.Register(ctx =>
        new AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ApiMappingProfile>();
        })
    ).AsSelf().SingleInstance();
});

// Додаємо контролери
builder.Services.AddControllers();

// Додаємо Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Додаємо CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Налаштування HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();

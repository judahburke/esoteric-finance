using Esoteric.Finance.Abstractions.Settings;
using Esoteric.Finance.Api.Handlers;
using Esoteric.Finance.Data;
using Esoteric.Finance.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(b => b.Filters.Add(
    new HttpStatusCodeExceptionFilter()
));
builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddLogging();
builder.Services.AddEsotericFinanceData();
builder.Services.AddEsotericFinanceServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

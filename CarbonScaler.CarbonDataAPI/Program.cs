using CarbonScaler.CarbonDataAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
});
builder.Services.AddHttpClient<ElectricityMapService>();
builder.Services.AddSingleton<ElectricityMapService>();

builder.Services.AddControllers();
builder.Services.AddHealthChecks();
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
app.MapHealthChecks("/health");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

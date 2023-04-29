using ApiApplication.Database;
using ApiApplication.Extensions;
using ApiApplication.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureLogging(logging =>
{
    logging.AddConsole();
});

builder.ConfigureInfrastructure()
    .ConfigureApplication();

builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddHttpClient();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExecutionTimeMiddleware>()
    .UseMiddleware<ErrorHandlerMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

SampleData.Initialize(app);

app.Run();
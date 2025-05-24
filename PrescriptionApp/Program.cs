using Microsoft.EntityFrameworkCore;
using PrescriptionApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("PrescriptionsDB"));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
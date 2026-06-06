using Szlakomat.Products.Infrastructure;
using Szlakomat.Parties.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddProductModule();
builder.Services.AddPartyModule();

var app = builder.Build();
app.MapControllers();
app.Run();

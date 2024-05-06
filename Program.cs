using chairs_dotnet7_api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("chairlist"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

var chairs = app.MapGroup("api/chair");

//TODO: ASIGNACION DE RUTAS A LOS ENDPOINTS
chairs.MapGet("/", GetChairs);

chairs.MapPost("/", AddChair);

chairs.MapGet("/{name}", GetChairByName);

chairs.MapPut("/{id}", UpdateChair);

chairs.MapPut("/{id}/stock", IncrementChairStock);

chairs.MapPost("/purchase", BuyChair);

chairs.MapDelete("/{id}", DeleteChairById);

app.Run();

//TODO: ENDPOINTS SOLICITADOS
static async Task<IResult> GetChairs(DataContext db)
{
    return TypedResults.Ok();
}

static async Task<IResult> AddChair(Chair chair, DataContext db)
{
    db.Chairs.Add(chair);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/chairs/{chair.Id}", chair);
}

static async Task<IResult> GetChairByName(String name, DataContext db)
{
    return TypedResults.Ok();
}

static async Task<IResult> UpdateChair(int id, DataContext db)
{
    return TypedResults.Ok();
}

static async Task<IResult> IncrementChairStock(int id, DataContext db)
{
    return TypedResults.Ok();
}

static async Task<IResult> BuyChair(int id, int ammount, DataContext db)
{
    return TypedResults.Ok();
}

static async Task<IResult> DeleteChairById(int id, DataContext db)
{
    return TypedResults.Ok();
}
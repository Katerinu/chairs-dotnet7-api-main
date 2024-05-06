using chairs_dotnet7_api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("chairlist"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

var chairs = app.MapGroup("api/chair");

//TODO: ASIGNACION DE RUTAS A LOS ENDPOINTS
chairs.MapGet("/", GetChairsAsync);

chairs.MapPost("/", AddChairAsync);

chairs.MapGet("/{name}", GetChairByNameAsync);

chairs.MapPut("/{id}", UpdateChairAsync);

chairs.MapPut("/{id}/stock", IncrementChairStockAsync);

chairs.MapPost("/purchase", BuyChairAsync);

chairs.MapDelete("/{id}", DeleteChairByIdAsync);

app.Run();

//TODO: ENDPOINTS SOLICITADOS
static async Task<IResult> GetChairsAsync(DataContext db)
{
    return TypedResults.Ok(await db.Chairs.ToArrayAsync());
}

static async Task<IResult> AddChairAsync(Chair chair, DataContext db)
{
    db.Chairs.Add(chair);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/chairs/{chair.Id}", chair);
}

static async Task<IResult> GetChairByNameAsync(String name, DataContext db)
{
    var foundChair = await db.Chairs.FirstOrDefaultAsync(x => x.Nombre == name);
    if(foundChair is null){
        return TypedResults.NotFound();
    }
    
    return TypedResults.Ok(foundChair);
}

static async Task<IResult> UpdateChairAsync(int id, Chair inputChair, DataContext db)
{
    var foundChair = await db.Chairs.FindAsync(id);
    if(foundChair is null)
    {
        return TypedResults.NotFound();
    }

    foundChair.Nombre = inputChair.Nombre;
    foundChair.Tipo = inputChair.Tipo;
    foundChair.Material = inputChair.Material;
    foundChair.Color = inputChair.Color;
    foundChair.Altura = inputChair.Altura;
    foundChair.Anchura = inputChair.Anchura;
    foundChair.Profundidad = inputChair.Profundidad;
    foundChair.Precio = inputChair.Precio;
    
    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}

static async Task<IResult> IncrementChairStockAsync(int id, Chair chairStock, DataContext db)
{
    var foundChair = await db.Chairs.FindAsync(id);
    if(foundChair is null)
    {
        return TypedResults.NotFound();
    }

    foundChair.Stock = foundChair.Stock + chairStock.Stock;
    await db.SaveChangesAsync();
    return TypedResults.Ok(foundChair);
}

static async Task<IResult> BuyChairAsync(int id, int ammount, int pricePayed, DataContext db)
{
    var foundChair = await db.Chairs.FindAsync(id);
    if(foundChair is null)
    {
        return TypedResults.NotFound();
    }

    

    return TypedResults.Ok();
}

static async Task<IResult> DeleteChairByIdAsync(int id, DataContext db)
{
    var foundChair = await db.Chairs.FindAsync(id);
    if(foundChair is null)
    {
        return TypedResults.NotFound();
    }

    db.Chairs.Remove(foundChair);
    await db.SaveChangesAsync();
    return TypedResults.NoContent();
}
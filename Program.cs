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
    return TypedResults.Ok(await db.Chairs.ToArrayAsync());
}

static async Task<IResult> AddChair(Chair chair, DataContext db)
{
    db.Chairs.Add(chair);
    await db.SaveChangesAsync();
    return TypedResults.Created($"/chairs/{chair.Id}", chair);
}

static async Task<IResult> GetChairByName(String name, DataContext db)
{
    var foundChair = await db.Chairs.FirstOrDefaultAsync(x => x.Nombre == name);
    if(foundChair is null){
        return TypedResults.NotFound();
    }
    
    return TypedResults.Ok(foundChair);
}

static async Task<IResult> UpdateChair(int id, Chair inputChair, DataContext db)
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

static async Task<IResult> IncrementChairStock(int id, int inputStock, DataContext db)
{
    var foundChair = await db.Chairs.FindAsync(id);
    if(foundChair is null)
    {
        return TypedResults.NotFound();
    }

    foundChair.Stock = foundChair.Stock + inputStock;
    await db.SaveChangesAsync();
    return TypedResults.Ok(foundChair);
}

static async Task<IResult> BuyChair(int id, int ammount, int pricePayed, DataContext db)
{
    var foundChair = await db.Chairs.FindAsync(id);
    if(foundChair is null)
    {
        return TypedResults.NotFound();
    }

    

    return TypedResults.Ok();
}

static async Task<IResult> DeleteChairById(int id, DataContext db)
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
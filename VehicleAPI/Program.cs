using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<VehicleDb>(options =>
    options.UseSqlite("Data Source=vehicles.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/vehicles", async (VehicleDb db) =>
{
    return await db.Vehicles.ToListAsync();
})
.WithName("GetVehicles");

app.MapGet("/vehicles/{Id}" , async (int Id, VehicleDb db) => 
{
    Vehicle vehicle = await db.Vehicles.FindAsync(Id);
    return vehicle is null ? Results.NotFound() : Results.Ok(vehicle); 
}).WithName("GetVehicleById");


app.MapPost("/vehicles",async (VehicleDb db, Vehicle vehicle) =>
{
    await db.Vehicles.AddAsync(vehicle); 
    await db.SaveChangesAsync();
    return  Results.Created($"/vehicle/{vehicle.Id}", vehicle); 
}).WithName("PostVehicle");

app.MapDelete("/vehicles/{Id}", async (VehicleDb db, int Id) =>
{
    Vehicle vehicle =await db.Vehicles.FindAsync(Id);
    if (vehicle is null) 
    { return Results.NotFound();}
    else
    { 
        db.Vehicles.Remove(vehicle);
        await db.SaveChangesAsync();
        return Results.NoContent();
    } 
}).WithName("RemvoeVehicle"); 

app.MapPut("/vehicles/{Id}",async (VehicleDb db, int Id, Vehicle vehicle) =>
{
    var existing = await db.Vehicles.FindAsync(Id);
    if( existing is null)
    {
        return Results.NotFound();     
    } else
    {
        existing.Make = vehicle.Make;
        existing.Model = vehicle.Model;
        existing.Year = vehicle.Year;
        existing.Type = vehicle.Type; 
        await db.SaveChangesAsync();
        return Results.Ok(existing);
    }
    
}).WithName("UpdateVehicle");

app.Run();



 class Vehicle
{
    public int Id {get; set;}
    public string Make {get; set;} = string.Empty;
    public string Model {get; set;} = string.Empty;
    public int Year {get; set;} 
    public string Type {get; set;} = string.Empty;

}


class VehicleDb : DbContext
{
    public VehicleDb(DbContextOptions<VehicleDb> options) : base(options) { }
    
    public DbSet<Vehicle> Vehicles { get; set; }
}
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

var vehicles = new List<Vehicle>
{
    new Vehicle { Id = 1, Make = "Toyota", Model = "Prius", Year = 2008, Type = "Car" },
    new Vehicle { Id = 2, Make = "Audi", Model = "A3", Year = 2014, Type = "Car" },
    new Vehicle { Id = 3, Make = "Yamaha", Model = "MT07", Year = 2019, Type = "Motorcycle" }
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/vehicles", () => vehicles)
.WithName("GetVehicles");

app.MapGet("/vehicles/{Id}" , (int Id) => 
{
    Vehicle vehicle = vehicles.FirstOrDefault(v => v.Id == Id);
    return vehicle is null ? Results.NotFound() : Results.Ok(vehicle); 
}).WithName("GetVehicleById");


app.MapPost("/vehicles", (Vehicle vehicle) =>
{
    vehicle.Id = vehicles.Max(v => v.Id) + 1;
    vehicles.Add(vehicle); 
    return  Results.Created($"/vehicle/{vehicle.Id}", vehicle); 
}).WithName("PostVehicle");

app.MapDelete("/vehicles/{Id}", (int Id) =>
{
    Vehicle vehicle = vehicles.FirstOrDefault(v => v.Id == Id);
    if (vehicle is null) 
    { return Results.NotFound();}
    else
    { 
        vehicles.Remove(vehicle);
        return Results.NoContent();
    } 
}).WithName("RemvoeVehicle"); 

app.MapPut("/vehicles/{Id}", (int Id, Vehicle vehicle) =>
{
    var existing = vehicles.FirstOrDefault(v => v.Id == Id);
    if( existing is null)
    {
        return Results.NotFound();     
    } else
    {
       existing.Make = vehicle.Make;
       existing.Model = vehicle.Model;
       existing.Year = vehicle.Year;
       existing.Type = vehicle.Type; 
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
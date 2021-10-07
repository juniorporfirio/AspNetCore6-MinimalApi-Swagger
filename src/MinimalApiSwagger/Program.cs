var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = builder.Environment.ApplicationName, Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1"));
}

app.MapGet("/customer/{id:guid}",
(Guid id) =>
{
    if (id == Guid.Empty)
        return Results.NotFound();

    return Results.Ok(new Customer(id, "Junior", "junior@porfirio.com"));
})
.Produces<Customer>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

app.MapPost("/customer",
(Customer customer) =>
{
    if (customer is null)
        return Results.NotFound();

    return Results.Created($"/customer/{customer.Id}", customer);
})
.Produces<Customer>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status404NotFound);

app.Run();
record Customer(Guid Id, string Name, string Email);

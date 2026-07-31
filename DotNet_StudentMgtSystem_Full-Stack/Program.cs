using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<StudentDbService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/signup", ([FromBody] UserAccount user, StudentDbService db) =>
{
    if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
    {
        return Results.BadRequest("Username and password are required.");
    }

    if (user.Role != "Teacher" && user.Role != "Admin")
    {
        return Results.BadRequest("Role must be Teacher or Admin.");
    }

    user.AccessCode = user.Role == "Teacher" ? "411" : "114";

    var signedUp = db.SignUp(user);
    return signedUp ? Results.Ok(new { message = "Sign up successful." }) : Results.Conflict("Username already exists.");
});

app.MapPost("/login", ([FromBody] LoginRequest request, StudentDbService db) =>
{
    var user = db.Login(request.Username, request.Password);
    if (user is null)
    {
        return Results.Unauthorized();
    }

    return Results.Ok(new { username = user.Username, role = user.Role, accessCode = user.AccessCode });
});

app.MapGet("/students", (StudentDbService db) =>
{
    return Results.Ok(db.GetAll());
});

app.MapGet("/students/{id:int}", (int id, StudentDbService db) =>
{
    var student = db.GetById(id);
    return student is null ? Results.NotFound() : Results.Ok(student);
});

app.MapPost("/students", ([FromBody] Student student, StudentDbService db) =>
{
    var created = db.Create(student);
    return Results.Created($"/students/{created.Id}", created);
});

app.MapPut("/students/{id:int}", (int id, [FromBody] Student student, StudentDbService db) =>
{
    if (id != student.Id)
    {
        return Results.BadRequest("Student ID mismatch.");
    }

    var updated = db.Update(student);
    return updated ? Results.Ok(student) : Results.NotFound();
});

app.MapDelete("/students/{id:int}", (int id, StudentDbService db) =>
{
    var deleted = db.Delete(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.Run();

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayRequestDuration();
    }
    );
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{
//endpoints.MapControllers(); // Map Controllers
//endpoints.MapRazorPages(); // Map Razor Pages

//// Custom endpoint to serve the CSHTML file
//endpoints.MapGet("/api/counter/view", async context =>
//{
//    await context.Response.SendFileAsync("Views/Counter.cshtml");
//});
//});
app.MapControllers();

app.Run();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// UseDefaultFiles y UseStaticFiles permiten que tu servidor sirva los archivos estáticos generados por React.
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();
// app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// MapFallbackToFile maneja las rutas desconocidas y las redirige a "/index.html",
// permitiendo que tu aplicación de página única maneje el enrutamiento.
app.MapFallbackToFile("/index.html");

app.Run();

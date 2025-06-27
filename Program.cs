using Microsoft.EntityFrameworkCore;
using ClinicaDentista.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados em memória
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ClinicaDentista"));

// Adiciona suporte a controllers com views (MVC)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Pipeline de requisição HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Importante para arquivos estáticos como CSS/JS

app.UseRouting();

app.UseAuthorization();

// Rota padrão MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

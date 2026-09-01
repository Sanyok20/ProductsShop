using Microsoft.EntityFrameworkCore;
using Products;
using Products.Initializer;
using Products.Repositories;
using Products.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    var connectionString = builder.Configuration.GetConnectionString("localDb");
    opt.UseNpgsql(connectionString);
});

builder.Services.AddScoped<CategoryRepository>();

builder.Services.AddScoped<ProductRepository>();

builder.Services.AddScoped<ImageService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>(); 

    await context.Database.ExecuteSqlRawAsync(@"
        SELECT setval(
          pg_get_serial_sequence('""Categories""', 'Id'), 
          COALESCE(MAX(""Id""), 1)
        ) FROM ""Categories"";
    ");

    await context.Database.ExecuteSqlRawAsync(@"
        SELECT setval(
          pg_get_serial_sequence('""Products""', 'Id'), 
          COALESCE(MAX(""Id""), 1)
        ) FROM ""Products"";
    ");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Seed();

app.Run();

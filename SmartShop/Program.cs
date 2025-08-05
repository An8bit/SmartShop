var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Thêm Razor Pages support
builder.Services.AddHttpClient("ApiClient");
builder.Services.AddScoped<SmartShop.Infrastructure.ApiClients.IApiClient, SmartShop.Infrastructure.ApiClients.ApiClient>();
builder.Services.AddScoped<SmartShop.Core.Interfaces.IProductService, SmartShop.Infrastructure.Services.ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Add this line to enable Razor Pages routing


app.Run();

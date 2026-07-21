using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string is missing");

//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddBLL(connectionString);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
   options => options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(4))
   .AddDefaultTokenProviders()
   .AddDefaultUI()
   .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";
    options.PopupRenderPosition = StackExchange.Profiling.RenderPosition.BottomLeft;
    options.PopupShowTimeWithChildren = true;
}).AddEntityFramework();


var app = builder.Build();
//await app.InitializeDatabaseAsync();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseMiniProfiler();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();



//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Account}/{action=Login}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();


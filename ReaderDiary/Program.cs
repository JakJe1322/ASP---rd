using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReaderDiary.Data;

var builder = WebApplication.CreateBuilder(args);

// Pøidání databázového kontextu
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Pøidání Identity s podporou rolí
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>() // <- pøidání rolí
.AddEntityFrameworkStores<ApplicationDbContext>();

// Pøidání MVC
builder.Services.AddControllersWithViews();

// Pøidání session
builder.Services.AddSession();

var app = builder.Build();

// Nastavení chování v režimu produkce
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Pøidání autentizace a autorizace
app.UseAuthentication();
app.UseAuthorization();

// Aktivace session middleware
app.UseSession();

// Nastavení routingu
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Dùležité pro Identity

// Vytvoøení výchozích rolí a admin úètu
await CreateRolesAndAdminUser(app);

app.Run();


// ------------------ VYTVOØENÍ ROLÍ A UŽIVATELÙ ------------------

async Task CreateRolesAndAdminUser(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // Role které chceme vytvoøit
    string[] roleNames = { "Admin", "User" };

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Vytvoøení admin úètu
    string adminEmail = "admin@example.com";
    string adminPassword = "Admin123!";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    // Vytvoøení bìžného uživatele
    string userEmail = "user@example.com";
    string userPassword = "User123!";

    var normalUser = await userManager.FindByEmailAsync(userEmail);
    if (normalUser == null)
    {
        normalUser = new IdentityUser
        {
            UserName = userEmail,
            Email = userEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(normalUser, userPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(normalUser, "User");
        }
    }
}

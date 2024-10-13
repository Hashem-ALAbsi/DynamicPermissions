using AutoMapper;
using DynamicPermissions.Models;
using DynamicPermissions.Configurations.Identity;
using DynamicPermissions.Configurations.Identity.PermissionManager;
using DynamicPermissions.Data;
using DynamicPermissions.Models;
using DynamicPermissions.Repository.AppSetting;
using DynamicPermissions.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Hosting;
using DynamicPermissions.App_Code;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<DynamicPermissionsContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//builder.Services.AddIdentityCore<User>();
builder.WebHost.ConfigureKestrel(opt => {
	opt.Limits.MaxRequestHeadersTotalSize = 1048576;
});
builder.Services.AddIdentity<ApplicationUser, Role>(options =>
{
	// Password settings.
	options.Password.RequireDigit = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 6;
	options.Password.RequiredUniqueChars = 1;

	// SignIn settings.
	options.SignIn.RequireConfirmedEmail = true;
	options.SignIn.RequireConfirmedPhoneNumber = false;

}).AddEntityFrameworkStores<DynamicPermissionsContext>()
				.AddDefaultTokenProviders()
				.AddEntityFrameworkStores<DynamicPermissionsContext>().AddDefaultUI()
				.AddErrorDescriber<PersianIdentityErrorDescriber>();
// AddIdentity<User,Role>(options => options.SignIn.RequireConfirmedAccount = true)
////.AddSignInManager<User>()
// .AddEntityFrameworkStores<DynamicPermissionsContext>().AddDefaultUI();
builder.Services.AddRazorPages();

//builder.Services.AddMvc(options =>
//{
//	options.Filters.Add(new AuthorizeFilter("DynamicPermission"));
//})
/*	.AddRazorPagesOptions(opt => {
	opt.Conventions.AddPageRoute("/Login", "");
})*/
	;

builder.Services.AddControllersWithViews()
	.AddRazorPagesOptions(opt =>
	{
		opt.Conventions.AddAreaPageRoute("Identity", "/Identity/Account/Login", "");
	})
	;
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSession();

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Login";
	options.LogoutPath = "/Identity/Account/LogOut";
	options.Cookie.Name = "IdentityCookie";
	//options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
});


builder.Services.Configure<SecurityStampValidatorOptions>(option =>
{
	option.ValidationInterval = TimeSpan.FromMinutes(20);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

builder.Services.AddAuthorization(option =>
{
	option.AddPolicy("DynamicPermission", policy => policy.Requirements.Add(new PermissionRequirement()));
});
//builder.Services.AddControllersWithViews(opt => opt.Filters.Add<PermissionAuthorizeAttribute>()).AddRazorRuntimeCompilation();

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(op => { op.LoginPath = "/Login"; });

builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddScoped<IAppSettingService, AppSettingService>();
builder.Services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();


//var mappingConfig = new MapperConfiguration(mc =>
//{

//});

//IMapper mapper = mappingConfig.CreateMapper();
//builder.Services.AddSingleton(mapper);
var app = builder.Build();
//app.Use(async (context, next) =>
//{
//if (!context.User.Identity.IsAuthenticated && context.Request.Path != "/Login")
//{
//	context.Response.Redirect("/Login");
//	return;
//}
//await next();
//});
// Configure the HTTP request pipeline.

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var loggerFactory = services.GetRequiredService<ILoggerFactory>();
	var logger = loggerFactory.CreateLogger("app");
	try
	{
		var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
		var roleManager = services.GetRequiredService<RoleManager<Role>>();
		var context = services.GetRequiredService<DynamicPermissionsContext>();
	
		await DynamicPermissions.Seeds.DefaultRoles.SeedAsync(userManager, roleManager);
		await DynamicPermissions.Seeds.DefaultUsers.SeedBasicUserAsync(userManager, roleManager);
		await DynamicPermissions.Seeds.DefaultUsers.SeedSuperAdminAsync(userManager, roleManager,context);
		logger.LogInformation("Finished Seeding Default Data");
		logger.LogInformation("Application Starting");
	}
	catch (Exception ex)
	{
		logger.LogWarning(ex, "An error occurred seeding the DB");
	}
}
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
	endpoints.MapRazorPages();
	endpoints.MapControllerRoute(name: "default", pattern: "{controller}/{action}/{id?}");
});
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=home}/{action=index}/{id?}");
app.MapRazorPages();

app.Run();

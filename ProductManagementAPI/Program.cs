
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Product.BL;
using Product.DAL;
using System.Security.Claims;
using System.Text;

namespace ProductManagementAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        #region services
        #region Database
        var ConnectionString = builder.Configuration.GetConnectionString("ConnectionDefault");
        builder.Services.AddDbContext<ProductContext>(o => o.UseSqlServer(ConnectionString));
        #endregion
        #region Identity Managers
        builder.Services.AddIdentity<Users, IdentityRole>(Options =>
        {
            Options.Password.RequireNonAlphanumeric = false;
            Options.Password.RequireUppercase = false;
            Options.Password.RequireLowercase = false;
            Options.Password.RequiredLength = 6;

            Options.User.RequireUniqueEmail = true;

            Options.Lockout.MaxFailedAccessAttempts = 4;
            Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
        }).AddEntityFrameworkStores<ProductContext>();


        #endregion


        #region Authentication
        builder.Services.AddAuthentication(Options =>
        {
            Options.DefaultAuthenticateScheme = "Default";
            Options.DefaultChallengeScheme = "Default";
        }).AddJwtBearer("Default", options =>
        {

            var SecretKey = builder.Configuration.GetValue<string>("Secret");
            var secretKeyInBytes = Encoding.ASCII.GetBytes(SecretKey);
            var key = new SymmetricSecurityKey(secretKeyInBytes);
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = key
            };

        });
        #endregion


        #region Authorization
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdministratorPolicy", p => p.RequireClaim(ClaimTypes.Role, "Administrator"));
            options.AddPolicy("ManagerPolicy", p => p.RequireClaim(ClaimTypes.Role, "Manager"));



        });
        #endregion

        builder.Services.AddScoped<IProductRepo, ProductRepo>();
        builder.Services.AddScoped<IProductManager, ProductManager>();
        #endregion


        builder.Services.AddControllers();
        /*   .ConfigureApiBehaviorOptions(o =>
           {
              o.SuppressModelStateInvalidFilter = true;
           });*/

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        #region MiddleWares
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();
        #endregion
        app.Run();

    }
}

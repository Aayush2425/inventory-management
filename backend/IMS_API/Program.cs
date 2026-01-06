using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IMS_API.Helper;
using IMS_API.Middleware;
using IMS_API.Repositories;
using IMS_API.Repositories.Interfaces;
using IMS_API.Services;
using IMS_API.Services.Interfaces;
using IMS_API.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);



// add repository scope
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();
builder.Services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();
builder.Services.AddScoped<ISalesOrderItemRepository, SalesOrderItemRepository>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddScoped<IPurchaseOrderItemRepository, PurchaseOrderItemRepository>();

//add service scope 
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IInventoryItemService, InventoryItemService>();
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
builder.Services.AddScoped<ISalesOrderItemService, SalesOrderItemService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<IPurchaseOrderItemService, PurchaseOrderItemService>();
builder.Services.AddScoped<IReportService, ReportService>();



// database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connString);
    options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
});

builder.Services.AddControllers();
// add helper scope
builder.Services.AddSingleton<JwtHelper>();
builder.Services.AddSingleton<PasswordHasher>();

builder.Services.AddEndpointsApiExplorer();

// jwt

var jwtSettings = builder.Configuration.GetSection("JWT");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = JwtRegisteredClaimNames.UniqueName

        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IMS API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your valid JWT token. Example: `Bearer eyJhbGciOiJIUzI1NiIs...`"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header
            },
            new string[] {}
        }
    });
});

// Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "IMS API v1");
    c.RoutePrefix = string.Empty;
});
app.UseCors();
app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


// "ConnectionStrings": {
//     "DefaultConnection": "Server=tcp:ims-sql-server-xyz.database.windows.net,1433;Initial Catalog=IMS_DB;Persist Security Info=False;User ID=sqladmin;Password=YOUR_PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
// }

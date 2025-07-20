using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // si tu utilises les cookies ou authentification
    });
});


// Add services to the container.

builder.Services.AddDbContext<PortailRHContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("PortailRHConnectionString"));
});

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);

    // Register the ValidationBehavior
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));

    // Add the LoggingBehavior to the MediatR pipeline
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddCarter();

builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ICongeRepository, CongeRepository>();
builder.Services.AddScoped<ICandidatureRepository, CandidatureRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IPaymentPolicyRepository, PaymentPolicyRepository>();
builder.Services.AddScoped<IPayslipRepository, PayslipRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<ISuiviCongeRepository, SuiviCongeRepository>();


builder.Services.AddAuthorization();

builder.Services.AddProblemDetails();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "PortailRH API", Version = "v1" });

    // Configuration pour le support JWT
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Entrez 'Bearer' [espace] puis votre token JWT.\n\nExemple : Bearer eyJhbGciOiJIUzI1NiIsInR5cCI..."
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var keyBytes = Convert.FromBase64String(builder.Configuration["Jwt:Key"]!);

        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            NameClaimType = ClaimTypes.Name
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Ajouter un admin par défaut si aucun admin n'existe en base
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PortailRHContext>();

    if (!context.Admins.Any())
    {
        var hasher = new PasswordHasher<Admin>();

        var admin = new Admin
        {
            Email = "admin@example.com"
        };

        admin.PasswordHash = hasher.HashPassword(admin, "Admin@123");

        context.Admins.Add(admin);
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAngularDevClient");
app.UseAuthentication(); 
app.UseAuthorization();



app.MapCarter();


app.Run();

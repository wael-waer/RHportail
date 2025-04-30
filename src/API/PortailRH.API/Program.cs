using PortailRH.API.Contracts;

var builder = WebApplication.CreateBuilder(args);

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
    
    // Add the ValidationBehavior to the MediatR pipeline
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


builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapCarter();
//app.MapGet("/", () => "Hello World!");
app.UseExceptionHandler(options => { });

app.Run();

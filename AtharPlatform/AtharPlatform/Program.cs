using AtharPlatform.Hubs;
using AtharPlatform.Repositories;
using AtharPlatform.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Inject Swagger
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<Context>(
//  options => options.UseNpgsql(builder.Configuration.GetConnectionString("connection"))
//  );
builder.Services.AddDbContext<Context>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


// Inject Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
builder.Services.AddScoped<ICharityRepository, CharityRepository>();
builder.Services.AddScoped<IDonorRepository, DonorRepository>();

// Inject Services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Inject Hubs
builder.Services.AddSignalR();
builder.Services.AddScoped<INotificationHub, NotificationHub>();

// Inject Identity
builder.Services
    .AddIdentity<UserAccount, IdentityRole<int>>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
});

// Inject JWT
builder.Services.AddAuthentication().AddJwtBearer();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//Nofications
app.MapHub<NotificationHub>("/notificationHub");

app.MapControllers();

app.Run();
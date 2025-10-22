using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// Inject DB
builder.Services.AddDbContext<Context>(
  options => options.UseNpgsql(builder.Configuration.GetConnectionString("connection"))
 );

// Inject Repositories


// Inject Services


// Inject Identity
builder.Services.AddIdentity<UserAccount, IdentityRole>();
builder.Services.Configure<IdentityOptions>(op =>
{
    op.Password.RequiredLength = 1;
    op.Password.RequireDigit = false;
    op.Password.RequireLowercase = false;
    op.Password.RequireUppercase = false;
    op.Password.RequireNonAlphanumeric = false;
    op.Password.RequiredUniqueChars = 0;
});

// Inject JWT
builder.Services.AddAuthentication().AddJwtBearer();

// Inject SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


// Nofications

app.MapControllers();

app.Run();


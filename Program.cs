
using api.Data;
using api.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// add Dependency Injection DI
builder.Services.AddApplicationServices();
// Identity JWT
builder.Services.AddIdentityServices();
// authentication middleware
builder.Services.AddAuthenticationServices(builder.Configuration);


// auto mappers
// builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add authen & author
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

// Add "api" prefix to all controllers
app.MapControllers();

app.Run();


using api.Data;
using api.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// AddSwaggerGen 
builder.Services.AddCustomSwaggerService();

// add Dependency Injection DI
builder.Services.AddApplicationServices();
// Identity JWT
builder.Services.AddIdentityServices();
// authentication
builder.Services.AddAuthenticationServices(builder.Configuration);


// auto mappers
// builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());




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

// handler error try catch in contrller ?
app.UseMiddleware<ErrorMiddlewareException>();

// Add authen & author as 1 middlleware
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

// Add "api" prefix to all controllers
app.MapControllers();

app.Run();

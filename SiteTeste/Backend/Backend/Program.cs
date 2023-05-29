using Backend.Data.ContactData;
using Backend.Data.SaveImageData;
using Backend.Data.UserLoginData;
using Backend.Data.UserRegistrationData;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Data base connection

var connectionString = builder.Configuration.GetConnectionString("DBConnection");

builder.Services.AddDbContext<ContactContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDbContext<SaveImageContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDbContext<UserLoginContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDbContext<UserRegistrationContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

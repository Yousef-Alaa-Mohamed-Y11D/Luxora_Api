
using Luxora_api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(option =>
{
    //option.ReturnHttpNotAcceptable=true)
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Add Services DbConetxt
builder.Services.AddDbContext<ApplicationDbContext>
    (option => option.UseSqlServer(builder.Configuration.GetConnectionString("CS")));
var app = builder.Build();

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

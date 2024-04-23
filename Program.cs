using ASAP_Clients.Base;
using ASAP_Clients.Data;
using ASAP_Clients.Manager;
using ASAP_Clients.Manager.IManager;
using ASAP_Clients.Repository;
using ASAP_Clients.Repository.IRepository;
using ASAP_Clients.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IClientsUnitOfWork, ClientsUnitOfWork>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IPolygonTickerRepository, PolygonTickerRepository>();
builder.Services.AddScoped<IPolygonRequestRepository, PolygonRequestRepository>();
builder.Services.AddScoped<IPreviousCloseResponseRepository, PreviousCloseResponseRepository>();

builder.Services.AddScoped<IPolygonManager, PolygonManager>();
builder.Services.AddScoped<IClientManager, ClientManager>();

builder.Services.AddHttpClient();

builder.Services.AddHostedService<PolygonDataService>();

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

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
builder.Services.AddCors(options =>
{
  options.AddPolicy("ASAP_Policy", builder =>
  {
    // Allow requests from your Angular application's domain
    builder.WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader(); // Replace with your actual domain

    // Optionally, specify allowed methods, headers, etc.
    //builder.WithMethods("GET", "POST", "PUT", "DELETE");
    // builder.WithHeaders("Content-Type", "Authorization");
  });
});

// var modelBuilder = new ODataConventionModelBuilder();
// modelBuilder.EntityType<ClientDto>();

// builder.Services.AddControllers().AddOData(
//     options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
//         "odata",
//         modelBuilder.GetEdmModel()));

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
builder.Services.AddScoped<IClientsMailManager, ClientsMailManager>();

builder.Services.AddHttpClient();

builder.Services.AddHostedService<PolygonDataService>();
builder.Services.AddHostedService<ClientsMailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ASAP_Policy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

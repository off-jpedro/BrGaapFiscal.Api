using BrGaapFiscal.Api.Repositores.Interfaces;
using BrGaapFiscal.Api.Repositores;
using BrGaapFiscal.Api.Services.Interfaces;
using BrGaapFiscal.Api.Services;
using Microsoft.EntityFrameworkCore;
using BrGaapFiscal.Infra.Data.Factory;
using BrGaapFiscal.Infra.Data.Repositores.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Adicionando o CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Permite qualquer origem
              .AllowAnyMethod()  // Permite qualquer método HTTP (GET, POST, etc.)
              .AllowAnyHeader(); // Permite qualquer cabeçalho
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<INotaFiscalRepository, NotaFiscalRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IFornecedorRepository, FornecedorRepository>();

builder.Services.AddScoped<INotaFiscalService, NotaFiscalService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IFornecedorService, FornecedorService>();

var app = builder.Build();

// Configuração do Swagger para desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adicionando o CORS na pipeline de requisição
app.UseCors("AllowAll");  // Essa linha permite que qualquer origem acesse a API

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

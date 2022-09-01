#region References
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Validator.Cliente.Models;
using Validator.Core.CustomEntities;
using Validator.Core.Interfaces;
using Validator.Core.Services;
using Validator.Infracturure.ConfigurationDB;
using Validator.Infracturure.Interfaces;
using Validator.Infracturure.Repositories;
using Validator.Infracturure.Tool.SHAs;
#endregion

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1.0", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Hash Converter API",
        Description = "Image to hash converter to validate if the original file was modified." +
        " Hash Converter API (SHA1, SHA256, SHA384 y SHA512) </br></br><u>Info:</u></br></br><b>Developer:</b> Darwin José Lugo Mota <b></br></br>Telefone:</b> +584126042751</br>",
        Contact = new OpenApiContact
        {
            //Name = "",
            Email = "darwin_lugo_mota@hotmail.com",
            //Url = new Uri("http://www.primercodigo.com")
        }
    });

    var baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
    var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    c.IncludeXmlComments(xmlPath);
    c.ResolveConflictingActions(a => a.First());
    c.OperationFilter<RemoveVersionFromParameter>();
    c.DocumentFilter<ReplaceVersionWithExactValuePath>();
});

var configuration = builder.Configuration;

builder.Services.AddSingleton<IDBHash, DBHash>();
builder.Services.AddTransient<IHashConvertServices, HashConvertServices>();
builder.Services.AddTransient<ISHA1, SHA1>();
builder.Services.AddTransient<ISHA256, SHA256>();
builder.Services.AddTransient<ISHA384, SHA384>();
builder.Services.AddTransient<ISHA512, SHA512>();
builder.Services.Configure<ConfigDBValidatorImege>(configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v1.0/swagger.json", "Hash Converter API V1.0");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

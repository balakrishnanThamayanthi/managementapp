using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(o => o.AddPolicy("AllowOrign", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Services.AddControllers();
//Enable CORS
/*builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrign", option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});*/

// JSON Serializer

builder.Services.AddControllersWithViews().AddNewtonsoftJson(Options => Options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson(Options => Options.SerializerSettings.ContractResolver = new DefaultContractResolver());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowOrign");
app.UseAuthorization();

app.MapControllers();

app.Run();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider =new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
    RequestPath = "/Photos"
});

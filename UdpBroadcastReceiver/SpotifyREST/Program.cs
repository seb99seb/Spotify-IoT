var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://zealand.dk").
        AllowAnyMethod().
        AllowAnyHeader()
    );
    options.AddPolicy("AllowAny",
        builder => builder.AllowAnyOrigin().
        AllowAnyMethod().
        AllowAnyHeader()
    );
    options.AddPolicy("AllowOnlyGetPut",
        builder => builder.AllowAnyOrigin().
        WithMethods("GET", "PUT").
        AllowAnyHeader()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowOnlyGetPut");

app.MapControllers();

app.Run();
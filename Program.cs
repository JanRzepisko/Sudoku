using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Sudoku.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.ConfigureSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Sudoku", Version = "v1" });
});
builder.Services.AddCors(options =>
{
  options.AddPolicy(name: "MyPolicy", policy  =>
  {
    policy
      .AllowCredentials()
      .AllowAnyMethod()
      .AllowAnyOrigin(); 
  });
});

var app = builder.Build();

app.UseSwagger(options => {
    options.SerializeAsV2 = true;
});

app.UseCors("MyPolicy");

app.UseSwaggerUI();
app.UseHttpsRedirection();

var game = new Game();

app.MapPost("/", ([FromBody]List<List<int>> input) => game.Init(input));
app.UseRouting();
app.Run();


/*
[
  [
    0,0,0,0,0,0,0,0,0
  ],  
  [
    0,0,0,0,0,0,0,0,0
  ],
  [
    0,0,0,0,0,0,0,0,0
  ],  
  [
    0,0,0,0,0,0,0,0,0
  ],  
  [
    0,0,0,0,0,0,0,0,0
  ],
  [
    0,0,0,0,0,0,0,0,0
  ],  
  [
    0,0,0,0,0,0,0,0,0
  ],  
  [
    0,0,0,0,0,0,0,0,0
  ],
  [
    0,0,0,0,0,0,0,0,0
  ]
]
*/
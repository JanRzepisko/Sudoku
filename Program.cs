using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Newtonsoft.Json;
using Sudoku.Models;
using Sudoku.Models.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll",
    policy =>
    {
      policy
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});


var app = builder.Build();
app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
var game = new Game();

app.MapPost("/", ([FromBody]List<List<int>> input) =>
{
  var res = game.Solve(input);
  Console.WriteLine(JsonConvert.SerializeObject(res));
  return res;
});

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
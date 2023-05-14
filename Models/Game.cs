using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Sudoku.Models.Interfaces;

namespace Sudoku.Models;

public class Game : IGame
{
    [MaxLength(9)] public IEnumerable<Row> Rows { get; private set; }
    [MaxLength(9)] public IEnumerable<Row> Columns { get; private set; }
    [MaxLength(9)] public IEnumerable<Row> Squares { get; private set; }
    [MaxLength(81)] public IEnumerable<Field> Fields { get; private set; }

    public void Solve()
    {
        throw new NotImplementedException();
    }

    public List<List<int>> Init(List<List<int>> input)
    {
        ImportRows(input);
        ImportColumns(input);
        ImportSquare(input);

        return Squares.Select(s => s.Fields.Select(f => f.Value).ToList()).ToList();
    }

    private void ImportRows(IReadOnlyList<IReadOnlyList<int>> input)
    {
        var rows = new List<Row>();
        for (var i = 0; i < 9; i++)
        {
            var row = new Row();
            var fields = new List<Field>();
            for (var j = 0; j < 9; j++)
            {
                var field = new Field()
                {
                    Value = input[i][j],
                    isOrginalValue = input[i][j] != 0,
                    FieldId = (((i + 1) * 9) + j) - 9
                };
                fields.Add(field);
            }

            row.Fields = fields;
            rows.Add(row);
        }

        Rows = rows;
    }
    private void ImportColumns(IReadOnlyList<IReadOnlyList<int>> input)
    {
        var columns = new List<Row>();
        for (var i = 0; i < 9; i++)
        {
            var column = new Row();
            var fields = new List<Field>();
            for (var j = 0; j < 9; j++)
            {
                var field = new Field()
                {
                    Value = input[j][i],
                    isOrginalValue = input[j][i] != 0,
                    FieldId = (((j + 1) * 9) + i) - 9
                };
                fields.Add(field);
            }

            column.Fields = fields;
            columns.Add(column);
        }

        Columns = columns;
    }
    private void ImportSquare(IReadOnlyList<IReadOnlyList<int>> input)
    {
        var squares = new List<List<Field>>();
        var bigSquares = new List<List<Field>>();
        var finalSquares = new List<List<Field>>();
        
        for (int i = 0; i < 3; i++)
        {
            bigSquares.Add(new List<Field>());
        }

        for (var i = 0; i < 9; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                squares.Add(Columns.ToList()[i].Fields.Skip(j * 3).Take(3).ToList());
            }
        }

        for (var i = 0; i < 27; i++)
        {
            bigSquares[i%3].AddRange(squares[i]);
        }

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 9; j++)
            {
                finalSquares.Add(bigSquares[i].Skip(j * 3).Take(3).ToList());
            }
        }
        
        var result = new List<Row>();
        
        for (var i = 0; i < 9; i++)
        {
            var fields = new List<Field>();
            for (var j = 0; j < 3; j++)
            {
                fields.AddRange(finalSquares[i * 3 + j]);
            }
            result.Add(new Row(){Fields = fields});
        }

        Squares = result.Select(x => new Row() {Fields = x.Fields.OrderBy(c => c.FieldId).ToList()});
    }
}
/*
[
  [1,2,3,0,0,0,1,2,3],  
  [4,5,6,0,0,0,6,5,4],
  [7,8,9,0,0,0,7,8,9],  
  [0,0,0,0,0,0,0,0,0],  
  [0,0,0,1,1,1,0,0,0],
  [0,0,0,0,0,0,0,0,0],  
  [0,0,0,0,0,0,0,0,0],  
  [0,0,0,0,0,0,0,0,0],
  [0,0,0,0,0,0,0,0,0]
]
*/

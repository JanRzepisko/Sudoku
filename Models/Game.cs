using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Sudoku.Models.Interfaces;

namespace Sudoku.Models;

public class Game : IGame
{
    [MaxLength(9)] private IEnumerable<Row>  Rows { get; set; }
    [MaxLength(9)] private IEnumerable<Row>  Columns { get; set; }
    [MaxLength(9)] private IEnumerable<Row>  Squares { get; set; }
    [MaxLength(81)] private IEnumerable<Field> Fields { get; set; }

    public IReadOnlyList<IReadOnlyList<int>> Solve(IReadOnlyList<IReadOnlyList<int>> input)
    {
        ImportData(input);

        while (Fields.Any(c => c.Value == 0))
        {
            var changes = 0;
            foreach (var field in Fields)
            {
                var canCheck = field is { Value: 0, isOrginalValue: false };
                if (!canCheck) continue;
                var possibleValues = new List<int>();

                var row = Rows.GetRowWithId(field.FieldId);
                var column = Columns.GetRowWithId(field.FieldId);
                var square = Squares.GetRowWithId(field.FieldId);

                for (var i = 1; i < 10; i++)
                {
                    if (row.CanAdd(i) && column.CanAdd(i) && square.CanAdd(i))
                        possibleValues.Add(i);
                }

                if (possibleValues.Count == 1)
                {
                    Console.WriteLine($"Added number: {possibleValues[0]} to field: {field.FieldId}");
                    SetNumber(field.FieldId, possibleValues[0]);
                    changes++;
                }
            }

            if (changes == 0)
            {
                break;
            }
        }



        return Rows.Select(c => c.Fields.Select(c => c.Value).ToList()).ToList();
    }
    public void SetNumber(int fieldId, int value)
    {
        foreach (var item in Columns)
        {   
            if (item.Fields.All(c => c.FieldId != fieldId)) continue;
            item.Fields.FirstOrDefault(c => c.FieldId == fieldId)!.Value = value;
            break;
        }

        foreach (var item in Rows)
        {
            if (item.Fields.All(c => c.FieldId != fieldId)) continue;
            item.Fields.FirstOrDefault(c => c.FieldId == fieldId)!.Value = value;
            break;
        }

        foreach (var item in Squares)
        {
            if (item.Fields.All(c => c.FieldId != fieldId)) continue;
            item.Fields.FirstOrDefault(c => c.FieldId == fieldId)!.Value = value;
            break;
        }

        foreach (var item in Fields)
        {
            if (fieldId == item.FieldId)
            {
                item.Value = value;
            }
        }
    }
    private void ImportData(IReadOnlyList<IReadOnlyList<int>> input)
    {
        ImportRows(input);
        ImportColumns(input);
        ImportSquare();
        ImportFields(input);
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

    private void ImportSquare()
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
            bigSquares[i % 3].AddRange(squares[i]);
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

            result.Add(new Row() { Fields = fields });
        }

        Squares = result.Select(x => new Row() { Fields = x.Fields.OrderBy(c => c.FieldId).ToList() });

    }

    private void ImportFields(IReadOnlyList<IReadOnlyList<int>> input)
    {
        var fields = new List<Field>();
        for (var i = 0; i < 9; i++)
        {
            for (var j = 0; j < 9; j++)
            {
                fields.Add(new Field()
                {
                    Value = input[j][i],
                    isOrginalValue = input[j][i] != 0,
                    FieldId = (((j + 1) * 9) + i) - 9
                });
            }
        }

        Fields = fields;
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

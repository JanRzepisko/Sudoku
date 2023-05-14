using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Sudoku.Models.Interfaces;

namespace Sudoku.Models;

public class Game : IGame
{
    public IEnumerable<Row>  Rows { get; set; }
    public IEnumerable<Row>  Columns { get; set; }
    public IEnumerable<Row>  Squares { get; set; }
    public  IEnumerable<Field> Fields { get; set; }

    public IReadOnlyList<IReadOnlyList<int>> Solve(IReadOnlyList<IReadOnlyList<int>> input)
    {
        this.ImportData(input);    
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
                else if (possibleValues.Count == 2)
                {
                    
                    
                    
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

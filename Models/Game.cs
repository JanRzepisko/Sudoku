using Microsoft.AspNetCore.Server.Kestrel.Transport.Quic;
using Sudoku.Models.Interfaces;

namespace Sudoku.Models;

public class Game : IGame
{
    public Game()
    {
        
    }
    public Game(Game game)
    {
        this.Columns = game.Columns;
        this.Rows = game.Rows;
        this.Squares = game.Squares;
        this.Fields = game.Fields;
    }
    public IEnumerable<Row>  Rows { get; set; }
    public IEnumerable<Row>  Columns { get; set; }
    public IEnumerable<Row>  Squares { get; set; }
    public IEnumerable<Field> Fields { get; set; }

    public IReadOnlyList<IReadOnlyList<int>> Solve(IReadOnlyList<IReadOnlyList<int>> input)
    {
        this.ImportData(input);    
        while (Fields.Any(c => c.Value == 0))
        {
            if (SolveNextQueue()) break;
        }
        return Rows.Select(c => c.Fields.Select(c => c.Value).ToList()).ToList();
    }

    private bool SolveWitchPremise(int fieldId, int value)
    {
        var localGame = new Game(this);
        localGame.SetNumber(fieldId, value);
        while (localGame.Fields.Any(c => c.Value == 0))
        {
            if (localGame.SolveNextQueue()) break;
        }

        return localGame.Fields.Any(c => c.Value == 0);
    }

    private bool SolveNextQueue()
    {
        var changes = 0;
        foreach (var field in Fields)
        {
            var canCheck = field is { Value: 0, isOrginalValue: false };
            if (!canCheck) continue;
            field.PossibleValues = new List<int>();
                
            var row = Rows.GetRowWithId(field.FieldId);
            var column = Columns.GetRowWithId(field.FieldId);
            var square = Squares.GetRowWithId(field.FieldId);

            for (var i = 1; i < 10; i++)
            {
                if (row.CanAdd(i) && column.CanAdd(i) && square.CanAdd(i))
                    field.PossibleValues.Add(i);
            }

            if (field.PossibleValues.Count == 1)
            {
                Console.WriteLine($"Added number: {field.PossibleValues[0]} to field: {field.FieldId}");
                SetNumber(field.FieldId, field.PossibleValues[0]);
                changes++;
            }
            else if (field.PossibleValues.Count == 2)
            {
                if (SolveWitchPremise(field.FieldId, field.PossibleValues[0]))
                {
                    SetNumber(field.FieldId, field.PossibleValues[0]);
                }
                else
                {
                    SetNumber(field.FieldId, field.PossibleValues[1]);
                }
                Console.WriteLine($"{field.PossibleValues[0]} {field.PossibleValues[1]} to field: {field.FieldId}");
            }
        }
        return changes == 0;
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
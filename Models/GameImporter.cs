namespace Sudoku.Models;

public static class GameImporter
{
    public static void ImportData(this Game game, IReadOnlyList<IReadOnlyList<int>> input)
    {
        ImportRows(game, input);
        ImportColumns(game, input);
        ImportSquare(game);
        ImportFields(game, input);
    }

    private static void ImportRows(this Game game, IReadOnlyList<IReadOnlyList<int>> input)
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

        
        game.Rows = rows;
    }

    private static void ImportColumns(this Game game, IReadOnlyList<IReadOnlyList<int>> input)
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

        game.Columns = columns;
    }

    private static void ImportSquare(this Game game)
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
                squares.Add(game.Columns.ToList()[i].Fields.Skip(j * 3).Take(3).ToList());
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

        game.Squares = result.Select(x => new Row() { Fields = x.Fields.OrderBy(c => c.FieldId).ToList() });

    }

    private static void ImportFields(this Game game, IReadOnlyList<IReadOnlyList<int>> input)
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

        game.Fields = fields;
    }
}
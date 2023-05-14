using System.Collections;
using Sudoku.Models.Interfaces;

namespace Sudoku.Models;

public static class RowListExtension
{
    public static Row GetRowWithId(this IEnumerable<Row> rows, int id)
    {
        foreach (var item in rows)
            if (item.Fields.Any(c => c.FieldId == id))
                return item;
        throw new Exception("Id not found");
    }
}
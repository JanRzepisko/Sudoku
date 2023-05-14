using Sudoku.Models.Interfaces;

namespace Sudoku.Models;

public class Row : IRow
{
    public IEnumerable<Field> Fields { get; set; }
    public bool CanAdd(int value) => !Fields.Any(v => v.Value == value);
}
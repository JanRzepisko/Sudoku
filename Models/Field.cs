namespace Sudoku.Models;

public class Field
{
    public int FieldId { get; init; }
    public int Value { get; set; }
    public bool isOrginalValue { get; init; }
}
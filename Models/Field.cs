namespace Sudoku.Models;

public class Field
{
    public Field(int fieldId)
    {
        FieldId = fieldId;
    }

    public int FieldId { get; }
    public int Value { get; set; }
}
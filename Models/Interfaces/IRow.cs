namespace Sudoku.Models.Interfaces;

public interface IRow
{
    public bool CanAdd(int value);
}
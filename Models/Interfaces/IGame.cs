namespace Sudoku.Models.Interfaces;

public interface IGame
{
    IReadOnlyList<IReadOnlyList<int>> Solve(IReadOnlyList<IReadOnlyList<int>> input);
    void SetNumber(int fieldId, int value);
}
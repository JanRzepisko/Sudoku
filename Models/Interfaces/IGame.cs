namespace Sudoku.Models.Interfaces;

public interface IGame
{
    void Solve();
    List<List<int>> Init(List<List<int>> input);
}
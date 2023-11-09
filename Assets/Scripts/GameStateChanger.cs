using UnityEngine;

public class GameStateChanger : MonoBehaviour
{
    public GameField GameField;
    public ShapeMover ShapeMover;

    private void Start()
    {
        FirstStartGame();
    }

    private void FirstStartGame()
    {
        GameField.FillCellsPositions();
        // это в качестве временного решения, чтобы выставить на окончании первого урока фигуру вверху поля
        ShapeMover.MoveShape(Vector2Int.right * (int)(GameField.FieldSize.x * 0.5f) + Vector2Int.up * (GameField.FieldSize.y - 2));
    }
}

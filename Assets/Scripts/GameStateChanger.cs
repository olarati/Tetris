using UnityEngine;

public class GameStateChanger : MonoBehaviour
{
    public GameField GameField;
    public ShapeMover ShapeMover;
    public ShapeSpawner ShapeSpawner;

    public GameObject GameScreen;
    public GameObject GameEndScreen;

    private void Start()
    {
        FirstStartGame();
    }

    public void SpawnNextShape()
    {
        Shape nextShape = ShapeSpawner.SpawnNextShape();
        ShapeMover.SetTargetShape(nextShape);
        ShapeMover.MoveShape(Vector2Int.right * (int)(GameField.FieldSize.x * 0.5f) + Vector2Int.up * (GameField.FieldSize.y - GameField.InvisibleYFieldSize));
    }

    public void EndGame()
    {
        SwitchScreens(false);
        ShapeMover.SetActive(false);
    }

    public void RestartGame()
    {
        ShapeMover.DestroyAllShapes();
        StartGame();
    }

    private void FirstStartGame()
    {
        GameField.FillCellsPositions();
        StartGame();
    }

    private void StartGame()
    {
        SpawnNextShape();
        SwitchScreens(true);
        ShapeMover.SetActive(true);
    }

    private void SwitchScreens(bool isGame)
    {
        GameScreen.SetActive(isGame);
        GameEndScreen.SetActive(!isGame);
    }

}

using TMPro;
using UnityEngine;

public class GameStateChanger : MonoBehaviour
{
    public GameField GameField;
    public ShapeMover ShapeMover;
    public ShapeSpawner ShapeSpawner;
    public Score Score;

    public GameObject GameScreen;
    public GameObject GameEndScreen;

    public TextMeshProUGUI GameEndScoreText;
    public TextMeshProUGUI BestScoreText;

    private void Start()
    {
        FirstStartGame();
    }

    public void SpawnNextShape()
    {
        Shape nextShape = ShapeSpawner.SpawnNextShape();
        ShapeMover.SetTargetShape(nextShape);
        ShapeMover.MoveShape(Vector2Int.right * (int)(GameField.FieldSize.x * 0.5f) + Vector2Int.up * (GameField.FieldSize.y - GameField.InvisibleYFieldSize + nextShape.ExtraSpawnYMove));
    }

    public void EndGame()
    {
        RefreshScores();
        SwitchScreens(false);
        ShapeMover.SetActive(false);
    }

    public void RestartGame()
    {
        Score.Restart();
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

    private void RefreshScores()
    {
        int score = Score.GetScore();
        int oldBestScore = Score.GetBestScore();
        bool isNewBestScore = CheckNewBestScore(score, oldBestScore);
        SetActiveGameEndScoreText(!isNewBestScore);
        if (isNewBestScore)
        {
            Score.SetBestScore(score);
            SetNewBestScoreText(score);
        }
        else
        {
            SetGameEndScoreText(score);
            SetOldBestScoreText(oldBestScore);
        }
    }

    private bool CheckNewBestScore(int score, int oldBestScore)
    {
        return score > oldBestScore;
    }

    private void SetGameEndScoreText(int value)
    {
        GameEndScoreText.text = $"Игра окончена!\nКоличество очков: {value}";
    }

    private void SetOldBestScoreText(int value)
    {
        BestScoreText.text = $"Лучший результат: {value}";
    }

    private void SetNewBestScoreText(int value)
    {
        BestScoreText.text = $"Новый рекорд: {value}!";
    }

    private void SetActiveGameEndScoreText(bool value)
    {
        GameEndScoreText.gameObject.SetActive(value);
    }

}

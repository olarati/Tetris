using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private const string BestScoreKey = "BestScore";

    private TextMeshProUGUI _scoreText;
    private Animation _scoreIncreaseAnimation; 
    private int _score;
    private int _bestScore;

    // Добавляем очки
    public void AddScore(int value)
    {
        SetScore(_score + value);
        PlayScoreIncreaseAnimation();
    }

    // Перезапускаем игру
    public void Restart()
    {
        SetScore(0);
    }

    // Получаем текущий счёт
    public int GetScore()
    {
        return _score;
    }

    // Получаем лучший счёт
    public int GetBestScore()
    {
        return _bestScore;
    }

    // Устанавливаем лучший счёт
    public void SetBestScore(int value)
    {
        _bestScore = value;
        SaveBestScore(value);
    }

    // Вызывается при запуске игры
    private void Start()
    {
        FillComponents();
        SetScore(0);
        LoadBestScore();
    }

    // Заполняем ссылки на компоненты
    private void FillComponents()
    {
        _scoreText = GetComponentInChildren<TextMeshProUGUI>();
        _scoreIncreaseAnimation = GetComponent<Animation>();
    }

    // Устанавливаем текущий счёт
    private void SetScore(int value)
    {
        _score = value;
        SetScoreText(value);
    }

    // Отображаем текст с очками
    private void SetScoreText(int value)
    {
        _scoreText.text = $"Очки: {value}";
    }

    // Загружаем лучший результат
    private void LoadBestScore()
    {
        _bestScore = PlayerPrefs.GetInt(BestScoreKey);
    }

    // Сохраняем лучший результат
    private void SaveBestScore(int value)
    {
        PlayerPrefs.SetInt(BestScoreKey, value);
    }
    private void PlayScoreIncreaseAnimation()
    {
        _scoreIncreaseAnimation.Play(PlayMode.StopAll);
    }
}

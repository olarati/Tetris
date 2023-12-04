using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public float ShownScoreChangeDuration = 0.2f;

    private const string BestScoreKey = "BestScore";

    private TextMeshProUGUI _scoreText;
    private Animation _scoreIncreaseAnimation; 
    private int _score;
    private int _bestScore;
    private int _shownScore;
    private int _shownScoreDelta;
    private bool _shownScoreChangeAnimationActive;
    private float _shownScoreChangeTimer;

    // ��������� ����
    public void AddScore(int value)
    {
        SetScore(_score + value);
        PlayScoreIncreaseAnimation();
    }

    // ������������� ����
    public void Restart()
    {
        SetScore(0);
        SetShownScore(0);
    }

    // �������� ������� ����
    public int GetScore()
    {
        return _score;
    }

    // �������� ������ ����
    public int GetBestScore()
    {
        return _bestScore;
    }

    // ������������� ������ ����
    public void SetBestScore(int value)
    {
        _bestScore = value;
        SaveBestScore(value);
    }

    // ���������� ��� ������� ����
    private void Start()
    {
        FillComponents();
        Restart();
        LoadBestScore();
    }

    // ��������� ������ �� ����������
    private void FillComponents()
    {
        _scoreText = GetComponentInChildren<TextMeshProUGUI>();
        _scoreIncreaseAnimation = GetComponent<Animation>();
    }

    // ������������� ������� ����
    private void SetScore(int value)
    {
        _score = value;
        ActivateShownScoreChangeAnimation();
    }

    // ���������� ����� � ������
    private void SetScoreText(int value)
    {
        _scoreText.text = $"����: {value}";
    }

    // ��������� ������ ���������
    private void LoadBestScore()
    {
        _bestScore = PlayerPrefs.GetInt(BestScoreKey);
    }

    // ��������� ������ ���������
    private void SaveBestScore(int value)
    {
        PlayerPrefs.SetInt(BestScoreKey, value);
    }
    private void PlayScoreIncreaseAnimation()
    {
        _scoreIncreaseAnimation.Play(PlayMode.StopAll);
    }

    private void ActivateShownScoreChangeAnimation()
    {
        _shownScoreChangeAnimationActive = true;
        _shownScoreChangeTimer = 0;
        _shownScoreDelta = _score - _shownScore;
    }

    private void DeactivateShownScoreChangeAnimation()
    {
        _shownScoreChangeAnimationActive = false;
    }

    private void Update()
    {
        ShownScoreChangeAnimation();
    }

    private void ShownScoreChangeAnimation()
    {
        if(!_shownScoreChangeAnimationActive)
        {
            return;
        }

        _shownScoreChangeTimer += Time.deltaTime / ShownScoreChangeDuration;
        int nextShownScore = (int) Mathf.Lerp(_shownScore,_score, _shownScoreChangeTimer);

        SetShownScore(nextShownScore);

        if(_shownScoreChangeTimer >= 1)
        {
            DeactivateShownScoreChangeAnimation();
        }
    }

    private void SetShownScore(int value)
    {
        _shownScore = value;
        SetScoreText(value);
    }
}

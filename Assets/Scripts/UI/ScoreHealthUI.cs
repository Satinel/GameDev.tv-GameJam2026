using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreHealthUI : MonoBehaviour
{
    [SerializeField] IntReferenceSO _scoreReference;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] GameObject[] _hearts;
    [SerializeField] Slider _timerSlider;

    void Awake()
    {
        ScoreKeeper.OnScoreChanged += UpdateScore;
        EelController.OnEelHealthChange += UpdateHealth;
        PemmingController.OnHealthChange += UpdatePemmingHealth;

        LevelTimer.OnTimerStarted += SetSliderMaxValue;
        LevelTimer.ReportCurrentTime += ChangeSliderValue;
    }

    void OnDestroy()
    {
        ScoreKeeper.OnScoreChanged -= UpdateScore;
        EelController.OnEelHealthChange -= UpdateHealth;
        PemmingController.OnHealthChange -= UpdatePemmingHealth;

        LevelTimer.OnTimerStarted -= SetSliderMaxValue;
        LevelTimer.ReportCurrentTime -= ChangeSliderValue;
    }

    void Start()
    {
        UpdateScore();
    }

    void UpdateScore()
    {
        _scoreText.text = $"- SCORE - {_scoreReference.Value:D3} -";
    }

    void UpdateHealth(int newHealth)
    {
        if(newHealth < 0) { return; }

        if(_hearts.Length > newHealth)
        {
            _hearts[newHealth].SetActive(false);
        }
    }

    void UpdatePemmingHealth(int newHealth)
    {
        if(newHealth < 0) { return; }

        for(int i = newHealth; i > 0; i--)
        {
            if(_hearts.Length > i)
            {
                _hearts[i].SetActive(true);
            }
        }
    }

    void SetSliderMaxValue(float value)
    {
        if(!_timerSlider) { return; }

        _timerSlider.gameObject.SetActive(true);
        _timerSlider.maxValue = value;
        _timerSlider.value = 0;
    }

    void ChangeSliderValue(float value)
    {
        if(!_timerSlider) { return; }

        _timerSlider.value = value;
    }
}

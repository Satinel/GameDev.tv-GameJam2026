using UnityEngine;
using TMPro;

public class ScoreHealthUI : MonoBehaviour
{
    [SerializeField] IntReferenceSO _scoreReference;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] GameObject[] _hearts;

    void Awake()
    {
        ScoreKeeper.OnScoreChanged += UpdateScore;
        EelController.OnEelHealthChange += UpdateHealth;
    }

    void OnDestroy()
    {
        ScoreKeeper.OnScoreChanged -= UpdateScore;
        EelController.OnEelHealthChange -= UpdateHealth;
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
}

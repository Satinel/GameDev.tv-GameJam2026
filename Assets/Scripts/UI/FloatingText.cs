using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] float _colorChangeDelay = 0.1f;

    float _timer;

    void Update()
    {
        _timer += Time.deltaTime;

        if(_timer >= _colorChangeDelay)
        {
            _text.color = Random.ColorHSV(0, 1, 0.75f, 0.8f, 0.75f, 0.8f);
            _timer -= _colorChangeDelay;
        }
    }

    public void SetTextFromInt(int value)
    {
        _text.color = Random.ColorHSV(0, 1, 0.75f, 0.8f, 0.75f, 0.8f);
        _text.text = $"{value}";
    }

    public void DestroySelfAnimationEvent()
    {
        Destroy(gameObject);
    }
}

using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    void Update()
    {
        _text.color = Random.ColorHSV(0, 1, 1, 1);
    }

    public void SetTextFromInt(int value)
    {
        _text.text = $"{value}";
    }

    public void DestroySelfAnimationEvent()
    {
        Destroy(gameObject);
    }
}

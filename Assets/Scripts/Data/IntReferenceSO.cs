using UnityEngine;

[CreateAssetMenu(fileName = "IntReferenceSO", menuName = "Scriptable Objects/IntReferenceSO")]
public class IntReferenceSO : ScriptableObject
{
    [field:SerializeField] public int Value { get; private set; }

    public void AddToValue(int amount)
    {
        Value += amount;
    }

    public void Reset()
    {
        Value = 0;
    }
}

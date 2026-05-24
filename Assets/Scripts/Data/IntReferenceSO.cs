using UnityEngine;

[CreateAssetMenu(fileName = "IntReferenceSO", menuName = "Scriptable Objects/IntReferenceSO")]
public class IntReferenceSO : ScriptableObject
{
    [field:SerializeField] public int Value { get; private set; }
    [field:SerializeField] public int ShopCost { get; private set; } = 1000;

    public void AddToValue(int amount)
    {
        Value += amount;
    }

    public void RemoveFromValue(int amount)
    {
        Value -= amount;
    }

    public void Reset()
    {
        Value = 0;
    }
}

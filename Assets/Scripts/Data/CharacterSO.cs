using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "Scriptable Objects/CharacterSO")]
public class CharacterSO : ScriptableObject
{
    [field:SerializeField] public string CharacterName { get; private set; }
    [field:SerializeField] public Sprite[] LeftSprites { get; private set; }
    [field:SerializeField] public Sprite[] RightSprites { get; private set; }
    [field:SerializeField] public AudioClip SoundFX { get; private set; }

}

using UnityEngine;

public class EelHome : MonoBehaviour, IElectrifiable
{
    [SerializeField] Switch[] _switchesArray;

    bool _isElectrified;

    public void Electrify()
    {
        if(_isElectrified) { return; }

        _isElectrified = true;
        foreach(Switch s in _switchesArray)
        {
            s.Electrify();
        }
    }

    public void Delectrify()
    {
        _isElectrified = false;
        foreach(Switch s in _switchesArray)
        {
            s.Delectrify();
        }
    }
}

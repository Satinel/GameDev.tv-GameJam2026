using UnityEngine;

public class EelHome : MonoBehaviour, IElectrifiable
{
    [SerializeField] Switch _switch;

    bool _isElectrified;

    public void Electrify()
    {
        if(_isElectrified) { return; }

        _isElectrified = true;
        _switch.Electrify();
    }

    public void Delectrify()
    {
        _isElectrified = false;
        _switch.Delectrify();
    }
}

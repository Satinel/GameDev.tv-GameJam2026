using UnityEngine;

public class EelHome : MonoBehaviour, IElectrifiable
{
    [SerializeField] GameObject _switch;

    bool _isElectrified;

    public void Electrify()
    {
        if(_isElectrified) { return; }

        _isElectrified = true;
        _switch.SetActive(false);
    }

    public void Delectrify()
    {
        _isElectrified = false;
        _switch.SetActive(true);
    }
}

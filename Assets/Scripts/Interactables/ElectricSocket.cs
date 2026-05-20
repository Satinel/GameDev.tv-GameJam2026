using UnityEngine;

public interface IElectrifiable
{
    public void Electrify();
    public void Delectrify();
}

public class ElectricSocket : MonoBehaviour
{
    [SerializeField] Animator _animator;

    static readonly int FLICKER_HASH = Animator.StringToHash("Flicker"), ZAP_HASH = Animator.StringToHash("Zap");

    float _timer;
    bool _isZapping;

    void Start()
    {
        _timer = Random.Range(7.5f, 45f);
    }

    void Update()
    {
        if(_isZapping) { return; }

        _timer -= Time.deltaTime;

        if(_timer <= 0)
        {
            _timer = Random.Range(7.5f, 45f);
            _animator.SetTrigger(FLICKER_HASH);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IElectrifiable electrifiable))
        {
            electrifiable.Electrify();
            _isZapping = true;
            _animator.SetBool(ZAP_HASH, _isZapping);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IElectrifiable electrifiable))
        {
            electrifiable.Delectrify();
            _isZapping = false;
            _animator.SetBool(ZAP_HASH, _isZapping);
        }
    }
}

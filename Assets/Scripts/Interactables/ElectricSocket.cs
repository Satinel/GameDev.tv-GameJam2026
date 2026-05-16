using UnityEngine;

public interface IElectrifiable
{
    public void Electrify();
    public void Delectrify();
}

public class ElectricSocket : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IElectrifiable electrifiable))
        {
            electrifiable.Electrify();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IElectrifiable electrifiable))
        {
            electrifiable.Delectrify();
        }
    }
}

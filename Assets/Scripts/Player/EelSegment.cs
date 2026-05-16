using UnityEngine;

public class EelSegment : MonoBehaviour
{
    [SerializeField] Collider2D _collider;

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<EelController>())
        {
            SetColliderNotTrigger();
        }
    }

    public void SetColliderNotTrigger()
    {
        _collider.isTrigger = false;
    }
}

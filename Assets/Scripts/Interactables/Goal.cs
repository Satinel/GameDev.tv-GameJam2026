using System;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static event Action OnGoalAchieved;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Pemming>())
        {
            OnGoalAchieved?.Invoke();
        }
    }
}

using System;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public event Action CollisionDetected;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out _))
        {
            CollisionDetected?.Invoke();
        }
    }
}

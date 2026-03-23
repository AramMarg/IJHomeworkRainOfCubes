using System;
using Random = System.Random;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Renderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Colorer _colorer;
    [SerializeField] private CollisionDetector _collisionDetector;

    private Random _random = new();
    private int _minTimeToDestroy = 2;
    private int _maxTimeToDestroy = 5;

    private bool _mustToColor = true;

    public event Action<Cube> ColorChanged;

    private void OnEnable()
    {
        _collisionDetector.CollisionDetected += OnCollisionDetected;
    }

    private void OnDisable()
    {
        _collisionDetector.CollisionDetected -= OnCollisionDetected;
    }

    private void OnCollisionDetected()
    {
        float timeToDestroy = _random.Next(_minTimeToDestroy, _maxTimeToDestroy);

        if (_mustToColor)
        {
            _colorer.ChangeColor(this);

            _mustToColor = false;

            Invoke(nameof(ReleaseCube), timeToDestroy);
        }
    }

    private void ReleaseCube()
    {
        _mustToColor = true;

        _colorer.ResetColor();

        ColorChanged?.Invoke(this);
    }
}


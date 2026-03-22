using System;
using Random = System.Random;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private Colorer _colorer = new();

    private Random _random = new();
    private int _minTimeToDestroy = 2;
    private int _maxTimeToDestroy = 5;

    private bool _mustToColor = true;
    private Renderer _cubeRenderer;
    private Color _initialColor;

    public event Action ColorChanged;

    private void Awake()
    {
        if (gameObject.TryGetComponent(out Renderer renderer))
        {
            _cubeRenderer = renderer;

            _initialColor = renderer.material.color;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out _))
        {
            float timeToDestroy = _random.Next(_minTimeToDestroy, _maxTimeToDestroy);

            if (_mustToColor)
            {
                _colorer.ChangeColor(_cubeRenderer);

                _mustToColor = !_mustToColor;

                Invoke(nameof(ReleaseCube), timeToDestroy);
            }
        }
    }

    private void ReleaseCube()
    {
        //_cubeRenderer.material.color = _initialColor;

        ColorChanged?.Invoke();

        Destroy(gameObject);
    }
}


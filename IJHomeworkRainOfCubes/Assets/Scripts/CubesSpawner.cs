using Random = System.Random;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubesSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _maxSize = 5;

    private Coroutine _coroutine;
    private float _delay = 0.5f;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>
        (
        createFunc: () => Instantiate(_cube),
        actionOnGet: (cube) => OnActionGet(cube),
        actionOnRelease: (cube) => OnActionRelease(cube),
        actionOnDestroy: (cube) => Destroy(cube),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _maxSize
        );
    }

    private void Start()
    {
        _coroutine = StartCoroutine(nameof(StartPooling));
    }
 
    private IEnumerator StartPooling()
    {
        WaitForSeconds wait = new(_delay);

        while (enabled)
        {
            _pool.Get();

            yield return wait;
        }
    }

    private void OnActionGet(Cube cube)
    {
        cube.transform.position = GetRandomPosition();

        if (cube.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.velocity = Vector3.zero;
        }

        cube.ColorChanged += OnColorChanged;

        cube.gameObject.SetActive(true);
    }

    private void OnActionRelease(Cube cube)
    {
        cube.gameObject.SetActive(false);
    }

    private void OnColorChanged(Cube cube)
    {
        cube.ColorChanged -= OnColorChanged;

        _pool.Release(cube);
    }

    private Vector3 GetRandomPosition()
    {
        int minForPosition = -14;
        int maxForPosition = 14;
        int tempForMaxBorder = 1;
        int maxForHeight = 15;

        Random random = new();

        return new Vector3(random.Next(minForPosition, maxForPosition + tempForMaxBorder),
            maxForHeight, random.Next(minForPosition, maxForPosition + tempForMaxBorder));
    }
}


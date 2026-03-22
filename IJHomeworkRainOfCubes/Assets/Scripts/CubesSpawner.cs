using Random = System.Random;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubesSpawner : MonoBehaviour
{
    [SerializeField] private Cube _forEvent;
    [SerializeField] private GameObject _cube;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _maxSize = 5;

    private Coroutine _coroutine;
    private float _delay = 0.5f;

    private ObjectPool<GameObject> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>
        (
        createFunc: () => Instantiate(_cube),
        actionOnGet: (cube) => ActionOnGet(cube),
        actionOnRelease: (cube) => ActionOnRelease(cube),
        actionOnDestroy: (cube) => Destroy(cube),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _maxSize
        );
    }

    private void OnEnable()
    {
        _forEvent.ColorChanged += OnColorChanged;
    }

    private void OnDisable()
    {
        _forEvent.ColorChanged -= OnColorChanged;
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

    private void ActionOnGet(GameObject cube)
    {
        cube.transform.position = GetRandomPosition();
        cube.GetComponent<Rigidbody>().velocity = Vector3.zero;
        cube.SetActive(true);
    }

    private void ActionOnRelease(GameObject cube)
    {
        cube.SetActive(false);
    }

    private void OnColorChanged()
    {
        _pool.Release(_cube);       
    }

    private Vector3 GetRandomPosition()
    {
        int minForPosition = -15;
        int maxForPosition = 15;
        int maxForHeight = 15;

        Random random = new();

        return new Vector3(random.Next(minForPosition, maxForPosition),
            maxForHeight, random.Next(minForPosition, maxForPosition));
    }
}


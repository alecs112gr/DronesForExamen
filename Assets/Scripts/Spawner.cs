using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private int _spawnDelay;
    [SerializeField] private Transform _spawnPosition1;
    [SerializeField] private Transform _spawnPosition2;
    [SerializeField] private Transform _container;
    [SerializeField] private int _maxResourcesCount;

    private void Start()
    {
        StartCoroutine(StartSpawning());
    }

    private void SpawnResource()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(_spawnPosition1.position.x, _spawnPosition2.position.x), //x
            _spawnPosition1.position.y, //y
            Random.Range(_spawnPosition1.position.z, _spawnPosition2.position.z)); //z

        Instantiate(_resourcePrefab, randomSpawnPosition, Quaternion.identity, _container);
    }

    private IEnumerator StartSpawning()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(_spawnDelay);

            if (_resourcePrefab != null && _container.childCount <= _maxResourcesCount)
            {
                SpawnResource();
            }         
        }
    }
}

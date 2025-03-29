using UnityEngine;

public class CreateDrone : MonoBehaviour
{
    [SerializeField] private Transform[] _freeMovingWays;
    [SerializeField] private Drone _dronePrefab;
    [SerializeField] private Transform _container;

    private Scanning _scanning;
    private BuildBase _buildBase;

    private void Start()
    {
        _scanning = GetComponent<Scanning>();
        _buildBase = FindFirstObjectByType<BuildBase>();

        for (int i = 0; i < 3; i++)
        {
            Create();
        }
    }

    public Transform[] GetFreeMovingWays()
    {
        return _freeMovingWays;
    }

    public void Create()
    {
        Transform pointOfSpawn = _freeMovingWays[Random.Range(0, _freeMovingWays.Length)];
        Quaternion rotation = pointOfSpawn.rotation;

        Drone drone = Instantiate(_dronePrefab, pointOfSpawn.position, rotation, _container);
        drone.Init(_freeMovingWays, _scanning, _buildBase);
        _scanning.AddDroneToList(drone);
    }
}

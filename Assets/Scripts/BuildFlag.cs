using UnityEngine;

public class BuildFlag : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private int _excludeDistanceFromBase;

    private float _yOffsetFlag = 0.9f;
    private int _leftMouseButton = 0;
    private BuildingState _buildingState;
    private BaseResources _baseResources;

    private void Awake()
    {
        _flagPrefab = Instantiate(_flagPrefab, Vector3.zero, Quaternion.Euler(0, 45, 0));
        _buildingState = GetComponent<BuildingState>();
        _baseResources = GetComponent<BaseResources>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(_leftMouseButton))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, _groundLayer) && Vector3.Distance(transform.position, hitInfo.point) >= _excludeDistanceFromBase)
            {
                if (_baseResources.GetResourcesCount() < 5) { return; }

                _baseResources.DecreaseResources(5);
                _flagPrefab.transform.position = hitInfo.point + (Vector3.up * _yOffsetFlag);
                _buildingState.OnFlagBuild(_flagPrefab);
                _baseResources.SetBuildingState(false);
                this.enabled = false;
            }
        }
    }
}

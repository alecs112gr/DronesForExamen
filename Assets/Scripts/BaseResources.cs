using UnityEngine;
using UnityEngine.UI;

public class BaseResources : MonoBehaviour
{
    [SerializeField] private Text _text;

    private CreateDrone _createDrone;
    private int _resources = 0;
    private int _startMaxResources = 3;
    private bool _isBuildingState;

    private void Start()
    {
        _createDrone = GetComponent<CreateDrone>();
    }

    public int GetResourcesCount()
    {
        return _resources;
    }

    public void SetBuildingState(bool value)
    {
        _isBuildingState = value;
        _startMaxResources = value ? 5 : 3;

        if (_resources > _startMaxResources && value == false)
        {
            CreateDroneForResources();
        }
    }

    public void AddResource()
    {
        _resources++;

        UpdateText();

        if (_resources >= _startMaxResources)
        {
            if (_isBuildingState == true)
            {
                return;
            }

            CreateDroneForResources();
        }
    }

    public void DecreaseResources(int value)
    {
        _resources -= value;
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = _resources.ToString();
    }

    private void CreateDroneForResources()
    {
        _createDrone.Create();
        _resources = 0;
        UpdateText();
    }
}

using System.Collections;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class BuildingState : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private bool _isSelected;
    private Color _startColor;
    private Scanning _scanning;
    private BuildFlag _buildFlag;
    private BaseResources _baseResources;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _startColor = _meshRenderer.material.color;
        _scanning = GetComponent<Scanning>();
        _buildFlag = GetComponent<BuildFlag>();
        _baseResources = GetComponent<BaseResources>();
    }

    public void OnFlagBuild(Flag flag)
    {
        foreach (Drone drone in _scanning.GetDrones())
        {
            if (drone.target == null && drone.goToBase == false && drone.goToFlag == false)
            {
                drone.GoFlagPosition(flag.transform);
                _isSelected = false;
                ChangeColor();
                Destroy(this);
                break;
            }
        }
    }

    private void ChangeColor()
    {
        _meshRenderer.material.color = _isSelected ? Color.red : _startColor;
    }

    private void SetBuildingStateToBase()
    {
        _baseResources.SetBuildingState(_isSelected);
    }

    private void SelectUnselectBase()
    {
        _isSelected = !_isSelected;
        ChangeColor();
        _buildFlag.enabled = _isSelected;

        SetBuildingStateToBase();
    }

    private void OnMouseUp()
    {
        SelectUnselectBase();
    }
}

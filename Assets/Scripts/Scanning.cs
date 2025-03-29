using System;
using System.Collections.Generic;
using UnityEngine;

public class Scanning : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _resourceLayerMask;

    private Queue<Resource> _resources = new Queue<Resource>();
    private List<Drone> _drones = new List<Drone>();

    public event Action OnResourcesChanged;

    private void Start()
    {
        PressToScanning.instance.AddBase(this);
    }

    public Queue<Resource> GetResources()
    {
        return _resources;
    }

    public Drone[] GetDrones()
    {
        return _drones.ToArray();
    }

    public void RemoveResourceFromQueue()
    {
        _resources.Dequeue();
    }

    public void AddDroneToList(Drone drone)
    {
        _drones.Add(drone);
    }

    public void RemoveDroneFromList(Drone drone)
    {
        if (!_drones.Contains(drone))
        {
            _drones.Remove(drone);
        }
    }

    private bool IsResourceFree(Resource resource)
    {
        return !resource.gameObject.CompareTag("Tagged");
    }

    private void TagResource(Collider resource)
    {
        resource.tag = "Tagged";
    }

    public void Scan()
    {
        print("Scanning...");
        Collider[] listResources = Physics.OverlapSphere(transform.position, _radius, _resourceLayerMask);

        foreach (Collider resource in listResources)
        {            
            Resource componentResource = resource.GetComponent<Resource>();

            if (!_resources.Contains(componentResource) && IsResourceFree(componentResource) == true)
            {
                TagResource(resource);
                _resources.Enqueue(componentResource);
            }
        }

        if (_resources.Count > 0)
        {
            OnResourcesChanged?.Invoke();
        }       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    private void OnDisable()
    {
        PressToScanning.instance.RemoveBase(this);
    }
}

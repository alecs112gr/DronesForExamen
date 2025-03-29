using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) //if resource spawned, then delete it
    {
        if (collision.gameObject.TryGetComponent(out Scanning scanning))
        {
            scanning.RemoveResourceFromQueue();
            Destroy(gameObject);
        }
    }
}

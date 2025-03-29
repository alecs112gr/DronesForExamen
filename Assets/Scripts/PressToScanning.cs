using System.Collections.Generic;
using UnityEngine;

public class PressToScanning : MonoBehaviour
{
    private List<Scanning> basesScanning = new List<Scanning>();

    public static PressToScanning instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void AddBase(Scanning _base)
    {
        if (basesScanning.Contains(_base) == false)
        {
            basesScanning.Add(_base);
        }        
    }

    public void RemoveBase(Scanning _base)
    {
        basesScanning.Remove(_base);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Scanning _base in basesScanning)
            {
                _base.Scan();
            }
        }
    }
}

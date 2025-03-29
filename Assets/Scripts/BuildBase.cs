using UnityEngine;

public class BuildBase : MonoBehaviour
{
    [SerializeField] private Scanning _base;

    private Scanning _tempBase;

    public void Build(Vector3 position)
    {
        _tempBase = Instantiate(_base, position, Quaternion.identity);
    }
}

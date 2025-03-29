using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    private const string Horizontal = nameof(Horizontal);
    private const string Vertical = nameof(Vertical);

    [SerializeField] private float _speed;

    private float _xAxis;
    private float _zAxis;

    private void LateUpdate()
    {
        _xAxis = Input.GetAxis(Horizontal);
        _zAxis = Input.GetAxis(Vertical);

        transform.Translate(_xAxis * _speed * Time.deltaTime, 0, _zAxis * _speed * Time.deltaTime, Space.World);
    }
}

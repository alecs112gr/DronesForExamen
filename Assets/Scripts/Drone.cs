using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform[] _freeMovingWays;
    [SerializeField] private Transform _resourcePosition;
    [SerializeField] private Scanning _base;
    [SerializeField] private BuildBase _buildBase;

    [HideInInspector] public Transform target { get; private set; }
    [HideInInspector] public bool goToBase { get; private set; }
    [HideInInspector] public bool goToFlag { get; private set; }

    private int _currentWay = 0;
    private float _rotationSpeed = 350;
    private Transform _flag;

    public void Init(Transform[] freeMovingWays, Scanning scanning, BuildBase buildBase)
    {
        _freeMovingWays = freeMovingWays;
        _base = scanning;
        _buildBase = buildBase;
        _base.OnResourcesChanged += SetTargetNextResource;
    }

    public void GoFlagPosition(Transform flag)
    {
        _flag = flag;
        goToFlag = true;
    }

    private Queue<Resource> TakeResourcesQueue()
    {
        return _base.GetResources();
    }

    private void SetTargetNextResource()
    {
        if (goToBase) { return; }
        if (target != null) { return; }
        if (TakeResourcesQueue().Count <= 0) { return; }

        target = TakeResourcesQueue().Dequeue().transform;
    }

    private void DoFreeMoving()
    {
        GoTarget(_freeMovingWays[_currentWay].position);

        if (transform.position == _freeMovingWays[_currentWay].position)
        {
            _currentWay = ++_currentWay % _freeMovingWays.Length;
        }      
    }

    private void GoTarget(Vector3 targetPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, _speed * Time.deltaTime);
        
        Vector3 directionToTarget = (targetPos - transform.position).normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(directionToTarget), _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target == null) { return; }

        if (other.gameObject.GetInstanceID() == target.gameObject.GetInstanceID())
        {
            goToBase = true;
            target = null;
            other.transform.SetParent(_resourcePosition);
            other.transform.position = _resourcePosition.position;
        }
    }

    private bool HaveResource()
    {
        return _resourcePosition.childCount > 0;
    }

    private Resource GetHavingResource()
    {
        return _resourcePosition.GetChild(0).GetComponent<Resource>();
    }

    private void GiveBaseResource(BaseResources baseResources)
    {
        baseResources.AddResource();
        Destroy(GetHavingResource().gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetInstanceID() == _base.gameObject.GetInstanceID())
        {
            goToBase = false;

            if (HaveResource())
            {
                GiveBaseResource(_base.GetComponent<BaseResources>());
            }        

            if (TakeResourcesQueue().Count <= 0)
            {
                return;
            }

            SetTargetNextResource();
        }
    }

    private void OnFlagEnter()
    {
        _buildBase.Build(new Vector3(transform.position.x, _base.transform.position.y, transform.position.z));
        Destroy(_flag.gameObject);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        if (_base == null) { return; }

        _base.OnResourcesChanged += SetTargetNextResource;
    }

    private void OnDisable()
    {
        _base.OnResourcesChanged -= SetTargetNextResource;
    }

    private void Update()
    {
        if (target != null)
        {
            GoTarget(target.position);
        }
        else if (goToBase)
        {
            GoTarget(_base.transform.position);
        }
        else if (goToFlag)
        {
            GoTarget(_flag.position);

            if (transform.position == _flag.position)
            {
                OnFlagEnter();
            }
        }
        else
        {
            DoFreeMoving();
        }
    }
}

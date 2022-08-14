using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;
        
    private Vector3 _zoomedTargetOffset;
    private Vector3 _deltaPosition;
    private bool _isZoomed;
    private float _speed = 4;    

    private void Start()
    {
        _zoomedTargetOffset = new Vector3(7, 4, 6);

        _deltaPosition = transform.position - _target.position;
    }

    private void Update()
    {
        if (_isZoomed == false)                    
            transform.position =  _target.position + _deltaPosition;        
        else        
            ShowFinish();        
    }

    public void ZoomFinish()
    {
        _isZoomed = true;
    }

    private void ShowFinish()
    {
        var rotationDirection = _target.position - transform.position;              

        transform.rotation = Quaternion.LookRotation(rotationDirection, Vector3.up);

        transform.position = Vector3.Lerp(transform.position, _target.position + _zoomedTargetOffset, Time.deltaTime * _speed);
    }
}
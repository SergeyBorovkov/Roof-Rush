using UnityEngine;
using DG.Tweening;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;
        
    private Vector3 _zoomedTargetOffset;
    private Vector3 _deltaPosition;
    private Vector3 _velocity = Vector3.zero;
    private bool _isZoomed;
    private float _smoothTime = 0.05f;    

    private void Start()
    {
        _zoomedTargetOffset = new Vector3(7, 4, 6);

        _deltaPosition = transform.position - _target.position;
    }

    private void Update()
    {
        if (_isZoomed == false)        
            transform.position = Vector3.SmoothDamp(transform.position, _target.position + _deltaPosition, ref _velocity, _smoothTime);        
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

        transform.DOMove(_target.position + _zoomedTargetOffset, 1f);
    }
}
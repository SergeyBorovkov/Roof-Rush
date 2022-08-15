using UnityEngine;
using DG.Tweening;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;
        
    private Vector3 _zoomedTargetOffset;
    private Vector3 _deltaPosition;    
    private bool _isZoomed;
    private float _durationY = 0.5f;
    private float _durationZ = 0.1f;

    private void Start()
    {
        _zoomedTargetOffset = new Vector3(7, 4, 6);

        _deltaPosition = transform.position - _target.position;
    }

    private void Update()
    {
        if (_isZoomed == false)
        {
            transform.DOMoveY(_target.position.y + _deltaPosition.y, _durationY);

            transform.DOMoveZ(_target.position.z + _deltaPosition.z, _durationZ);
        }
        else
        {
            ShowFinish();
        }
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
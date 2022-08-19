using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _noticeImage;
    [SerializeField] private float _startPointZ;
    [SerializeField] private float _endPointZ;    
        
    private Sequence _noticeAnimationSequience;
    private Camera _camera;
    private float _totalDistance;    
    private float _noticeCenterPointX;
    private float _noticeEndPointX;    
    private float _noticeStartPointX = -150;
    private float _noticeMoveDuration = 0.3f;
    private float _noticeRotateDuration = 0.3f;
    private Vector3 _noticeRotated = new Vector3 (0, 0, 30);

    private void Start()
    {
        _totalDistance = _endPointZ - _startPointZ;

        _camera = Camera.main;      
    }

    private void Update()
    {
        if (_player.transform.position.z > _endPointZ)        
            _slider.value = 1;        
        else        
            _slider.value = _player.transform.position.z / _totalDistance;        
    }

    public void AnimateNotice()
    {
        StartAnimation();        
    }

    private void StartAnimation()
    {
        ResetNoticePosition();

        _noticeCenterPointX = _camera.pixelWidth * 0.5f;

        _noticeEndPointX = Camera.main.pixelWidth - _noticeStartPointX;

        _noticeAnimationSequience = DOTween.Sequence();

        _noticeAnimationSequience.Append(_noticeImage.transform.DOMoveX(_noticeCenterPointX, _noticeMoveDuration).SetEase(Ease.InOutQuint));
        _noticeAnimationSequience.Append(_noticeImage.transform.DORotate(_noticeRotated, _noticeRotateDuration, RotateMode.Fast).SetEase(Ease.Linear));
        _noticeAnimationSequience.Append(_noticeImage.transform.DORotate(Vector3.zero, _noticeRotateDuration, RotateMode.Fast).SetEase(Ease.Linear));
        _noticeAnimationSequience.Append(_noticeImage.transform.DOMoveX(_noticeEndPointX, _noticeMoveDuration).SetEase(Ease.OutQuint));        
    }

    private void ResetNoticePosition()
    {
        _noticeImage.transform.position = new Vector3(_noticeStartPointX, _noticeImage.transform.position.y, _noticeImage.transform.position.z);        
    }  
}
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    private Sequence _scalingSequence;
    private Vector3 _defaultScale;
    private Vector3 _defaultRotation;
    private float _rotationTime = 1;
    private float _scaleTime = 0.2f;
    private float _decreaseScaleRatio = 0.11f;
    private float _increaseScaleRatio = 2f;    

    private void Start()
    {
        _defaultScale = transform.localScale;

        _defaultRotation = transform.eulerAngles;

        RotateOnY();        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())        
            ScaleToDestroy();        
    }

    private void RotateOnY()
    {
        transform.DORotate(new Vector3(_defaultRotation.x, 360, _defaultRotation.z), _rotationTime, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }  

    private void ScaleToDestroy()
    {
        _scalingSequence = DOTween.Sequence();

        _scalingSequence.Append(transform.DOScale(_defaultScale * _increaseScaleRatio, _scaleTime));
        _scalingSequence.Append(transform.DOScale(_defaultScale * _decreaseScaleRatio, _scaleTime));
        _scalingSequence.AppendCallback(() => Destroy(gameObject));
    }
}
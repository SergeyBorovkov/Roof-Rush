using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour
{
    [SerializeField] private Ground _ground;    
    [SerializeField] private bool _isGoodNoticed;    
    [SerializeField] private float _jumpModifier;
    
    private Sequence _scalingSequence;    
    private float _defaultScaleX;    
    private bool _isPlayerEntered;
    private bool _isWaveActivated;
    private float _shortPause = 0.1f;
    private float _longPause = 0.2f;    
    private float _maxScaleX = 3;
    private float _scaleTime = 0.3f;

    public Ground Ground => _ground;
    public bool IsGoodNoticed => _isGoodNoticed;
    public bool IsPlayerEntered => _isPlayerEntered;
    public float JumpModifier => _jumpModifier;

    private void Start()
    {
        _defaultScaleX = transform.localScale.x;        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player))
        {
            _isPlayerEntered = true;

            if (player.IsGrounded == false)
                ActivateLongWave();
        }
    }

    public void ActivateShortWave()
    {
        ScaleWave(_shortPause);        
    }

    public void ActivateLongWave()
    {
        ScaleWave(_longPause);        
    }

    private void ScaleWave(float pause)
    {
        if (_isWaveActivated == false)
        {
            _scalingSequence = DOTween.Sequence();

            _scalingSequence.Append(transform.DOScaleX(_maxScaleX, _scaleTime));
            _scalingSequence.AppendInterval(pause);
            _scalingSequence.Append(transform.DOScaleX(_defaultScaleX, _scaleTime));

            _isWaveActivated = true;
        }
    }    
}
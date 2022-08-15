using System;
using UnityEngine;
using DG.Tweening;

public class Block : MonoBehaviour
{
    [SerializeField] private Ground _ground;    
    [SerializeField] private bool _isGoodNoticed;    
    [SerializeField] private float _jumpModifier;

    public event Action <Block> IsJumpActivated;

    private Sequence _scalingSequence;    
    private float _defaultScaleX;    
    private float _pause = 0.1f;
    private float _longPause = 3f;    
    private float _maxScaleX = 3;
    private float _scaleTime = 0.3f;

    public Ground Ground => _ground;
    public bool IsGoodNoticed => _isGoodNoticed;
    public float JumpModifier => _jumpModifier;

    private void Start()
    {
        _defaultScaleX = transform.localScale.x;        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Player>(out Player player) && _ground.IsPlayerOnBlocks)
        {
            if (player.IsGrounded == false)            
                ScaleWave(_longPause);            
            else                         
                ScaleWave(_pause);                                        
        }
    }

    public void TriggerJumpActivatedEvent()
    {
        IsJumpActivated?.Invoke(this);
    }

    public void ActivateWave()
    {        
        ScaleWave(_pause);
    }
    
    private void ScaleWave(float pause)
    {
        _scalingSequence = DOTween.Sequence();

        _scalingSequence.Append(transform.DOScaleX(_maxScaleX, _scaleTime));
        _scalingSequence.AppendInterval(pause);
        _scalingSequence.Append(transform.DOScaleX(_defaultScaleX, _scaleTime));
    }    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{    
    [SerializeField] private Player _player;
    [SerializeField] private List<Block> _blocks;
    [SerializeField] private UIHandler _UIhandler;
    [SerializeField] private ParticleSystem _yellowEffect;
    [SerializeField] private ParticleSystem _redEffect;

    private bool _isScaleEnded;
    private Vector3 _playEffectPosition;
    private Block _jumpingBlock;
    private Coroutine _changeScaleJob;
    private bool _isPlayerOnBlocks;    
    private bool _isNoticeShown;
    private float _resetTime = 1f;
    private WaitForSeconds _scalePause = new WaitForSeconds(0.005f);

    public bool IsScaleEnded => _isScaleEnded;

    public bool IsPlayerOnBlocks => _isPlayerOnBlocks;

    private void OnEnable()
    {
        _player.IsJumped += OnPlayerJumped;

        foreach (var block in _blocks)
        {
            block.IsJumpActivated += OnJumpActivated;
        }
    }

    private void OnDisable()
    {
        _player.IsJumped -= OnPlayerJumped;

        foreach (var block in _blocks)
        {
            block.IsJumpActivated -= OnJumpActivated;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())        
            _isPlayerOnBlocks = true;                    
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())        
            _isPlayerOnBlocks = false;        
    }

    private void OnPlayerJumped(Block jumpingBlock)
    {
        if (_isPlayerOnBlocks && jumpingBlock != null)
        {            
            if (_changeScaleJob != null)
            {
                StopCoroutine(_changeScaleJob);
                _changeScaleJob = null;
            }
            else
            {
                _changeScaleJob = StartCoroutine(MakeScaleWithPause(GetRestBlocks(_blocks, jumpingBlock), _scalePause));           
            }
        }
    }

    private List<Block> GetRestBlocks(List<Block> initialBlocks, Block afterBlockExclusive)
    {
        bool isStartCopy = false;

        List<Block> restBlocks = new List<Block>();

        foreach (var block in initialBlocks)
        {
            if (isStartCopy)
                restBlocks.Add(block);

            if (block == afterBlockExclusive)
                isStartCopy = true;
        }

        return restBlocks;
    }    

    private IEnumerator MakeScaleWithPause(List<Block> blocks, WaitForSeconds pause)
    {
        foreach (var block in blocks)
        {
            block.ActivateWave();
            yield return pause;
        }

        _isScaleEnded = true;

        Invoke(nameof(SetFalse), _resetTime);
    }

    private void SetFalse()
    {
        _isScaleEnded = false;
    }

    private void OnJumpActivated(Block block)
    {
        _jumpingBlock = block;

        if (_isNoticeShown == false)
        {
            _playEffectPosition = new Vector3(0, 0, block.transform.position.z);

            if (block.IsGoodNoticed)
            {
                _UIhandler.ShowNotice();

                PlayEffect(_yellowEffect, _playEffectPosition);
            }
            else
            {
                PlayEffect(_redEffect, _playEffectPosition);
            }            

            _isNoticeShown = true;
        }

        Invoke(nameof(ResetNoticeShowing), _resetTime);
    }

    private void PlayEffect(ParticleSystem effect, Vector3 position)
    {
        effect.transform.position = position;
        effect.Play();
    }

    private void ResetNoticeShowing()
    {
        _isNoticeShown = false;
    }   
}
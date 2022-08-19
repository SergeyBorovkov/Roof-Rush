using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{    
    [SerializeField] private Player _player;
    [SerializeField] private List<Block> _blocks;
    [SerializeField] private UIHandler _UIHandler;
    [SerializeField] private ParticleSystem _yellowEffect;
    [SerializeField] private ParticleSystem _redEffect;

    private Vector3 _playEffectPosition;
    private Block _jumpingBlock;
    private Coroutine _changeScaleJob;
    private bool _isScaleStarted;
    private bool _isPlayerOnBlocks;        
    private float _resetScalingTime = 1f;           

    private void OnEnable()
    {
        _player.IsJumpedFromBlock += OnPlayerJumpedFromBlock;
    }

    private void OnDisable()
    {
        _player.IsJumpedFromBlock -= OnPlayerJumpedFromBlock;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            _isPlayerOnBlocks = true;

            foreach (var block in _blocks)
            {
                if (block.IsPlayerEntered)
                    block.ActivateShortWave();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())        
            _isPlayerOnBlocks = false;        
    }

    private void OnPlayerJumpedFromBlock(Block jumpingBlock)
    {
        if (_isPlayerOnBlocks && jumpingBlock != null)
        {            
            if (_changeScaleJob != null)
            {
                StopCoroutine(_changeScaleJob);
                _changeScaleJob = null;
            }
            
            _changeScaleJob = StartCoroutine(MakeLongWave(GetRestBlocks(_blocks, jumpingBlock)));            
        
            ShowNotice(jumpingBlock);

            ShowEffect(jumpingBlock);
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

    private IEnumerator MakeLongWave(List<Block> blocks)
    {
        _isScaleStarted = true;

        foreach (var block in blocks)
        {
            block.ActivateLongWave();
            yield return null;
        }

        Invoke(nameof(SetScalingFalse), _resetScalingTime);
    }

    private void SetScalingFalse()
    {
        _isScaleStarted = false;
    }

    private void ShowNotice(Block block)
    {
        if (block.IsGoodNoticed)
            _UIHandler.AnimateNotice();        
    }

    private void ShowEffect(Block block)
    {
        if (block.IsGoodNoticed)        
            PlayEffect(_yellowEffect);        
        else        
            PlayEffect(_redEffect);        
    }

    private void PlayEffect(ParticleSystem effect)
    {
        effect.transform.position = new Vector3(0,0, _player.transform.position.z);
        effect.Play();
    }  
}
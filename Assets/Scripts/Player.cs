using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpLength;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private CameraFollower _cameraFollower;

    public event Action <Block>IsJumped;

    private Sequence _jumpSequence;
    private Ground _ground;
    private Block _block;
    private int _isGroundedHash = Animator.StringToHash("IsGrounded");
    private int _jumpTriggerHash = Animator.StringToHash("JumpTrigger");
    private int _isFinishedHash = Animator.StringToHash("IsFinished");
    private bool _isGrounded;    
    private bool _isFinished;
    private bool _isCheckingGround = true;
    private float _defaultJumpSpeed;    
    private float _defaultHeight;    
    private float _jumpModifier = 1;

    public bool IsGrounded => _isGrounded;

    private void Start()
    {
        _defaultHeight = transform.position.y;        

        _isGrounded = true;        

        _defaultJumpSpeed = _jumpLength;        
    }

    private void Update()
    {       
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _isCheckingGround = false;

            _isGrounded = false;

            _animator.SetTrigger(_jumpTriggerHash);            

            if (_block != null)
            {
                _block.TriggerJumpActivatedEvent();
                StartCoroutine(ActivateJump());                
            }
            else
            {
                Jump(_jumpHeight, _jumpLength, _jumpModifier);                
            }

            IsJumped?.Invoke(_block);            
        }

        if (_isGrounded)        
            transform.position = new Vector3(0, _defaultHeight, transform.position.z + Time.deltaTime * _runSpeed);        

        _animator.SetBool(_isGroundedHash, _isGrounded);        
    }

    private void FixedUpdate()
    {
        if (_isCheckingGround)
        {
            if (Physics.CheckSphere(transform.position + new Vector3(0, 0.9f, 0), 1.1f, _groundLayer))            
                _isGrounded = true;            
            else         
                _isGrounded = false;         
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Ground>(out Ground ground))        
            _ground = ground;        

        if (other.gameObject.TryGetComponent<BreakableWindow>(out BreakableWindow window))        
            window.breakWindow();        

        if (other.gameObject.GetComponent<Finisher>())
        {
            _isGrounded = false;

            _isFinished = true;

            _rigidbody.isKinematic = true;

            _animator.SetBool(_isFinishedHash, _isFinished);

            _cameraFollower.ZoomFinish();
        }

        if (other.gameObject.TryGetComponent<Block>(out Block block))
        {
            _jumpModifier = block.JumpModifier;

            _block = block;

            _ground = block.Ground;
        }
    }

    private IEnumerator ActivateJump()
    {
        yield return new WaitUntil(() => _ground.IsScaleStarted);

        Jump(_jumpHeight, _jumpLength, _jumpModifier);
    }

    private void Jump(float height, float length, float multiplier)
    {
        float upTime = 0.6f;
        float downTime = 0.5f;
        float heightRatio = 1;

        if (multiplier > 1)
        {
            float newHeightRatio = 1 / (float)Math.Sqrt(multiplier);

            heightRatio = newHeightRatio < heightRatio ? newHeightRatio : heightRatio;            
        }

        _jumpSequence = DOTween.Sequence();

        _jumpSequence.Append(transform.DOMoveY(transform.position.y + height * multiplier * heightRatio, upTime * multiplier * heightRatio).SetEase(Ease.InOutQuad));
        _jumpSequence.Append(transform.DOMoveY(_defaultHeight, downTime * multiplier * heightRatio).SetEase(Ease.InSine));
        _jumpSequence.Insert(0, transform.DOMoveZ(transform.position.z + length * multiplier, (upTime + downTime) * multiplier * heightRatio).SetEase(Ease.Linear));
        _jumpSequence.AppendCallback(EndJumping);
    }
    
    private void EndJumping()
    {
        _jumpLength = _defaultJumpSpeed;

        _jumpModifier = 1;

        _block = null;

        _isCheckingGround = true;
    }
}
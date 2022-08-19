using System;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private FinishCamera _finishCamera;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpLength;
    [SerializeField] private float _jumpHeight;

    public event Action <Block>IsJumpedFromBlock;

    private Sequence _jumpSequence;    
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
    private float _spherePositionY = 0.9f;
    private float _sphereRadius = 1.1f;

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
                IsJumpedFromBlock?.Invoke(_block);            
            
            Jump(_jumpHeight, _jumpLength, _jumpModifier);                
        }

        if (_isFinished == false)            
            transform.position = new Vector3(0, _defaultHeight, transform.position.z + Time.deltaTime * _runSpeed);        

        _animator.SetBool(_isGroundedHash, _isGrounded);        
    }

    private void FixedUpdate()
    {
        if (_isCheckingGround)
        {
            if (Physics.CheckSphere(transform.position + new Vector3(0, _spherePositionY, 0), _sphereRadius, _groundLayer))            
                _isGrounded = true;            
            else         
                _isGrounded = false;         
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<BreakableWindow>(out BreakableWindow window))        
            window.breakWindow();        

        if (other.gameObject.GetComponent<FinishPlatform>())
        {
            _isGrounded = false;

            _isFinished = true;

            _rigidbody.isKinematic = true;

            _animator.SetBool(_isFinishedHash, _isFinished);

            _finishCamera.Play();
        }

        if (other.gameObject.TryGetComponent<Block>(out Block block))
        {
            _jumpModifier = block.JumpModifier;

            _block = block;            
        }
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

        _jumpSequence.Append(transform.DOMoveY(transform.position.y + height * multiplier * heightRatio, upTime * multiplier * heightRatio).SetEase(Ease.OutSine));
        _jumpSequence.Append(transform.DOMoveY(_defaultHeight, downTime * multiplier * heightRatio).SetEase(Ease.InSine));
        _jumpSequence.Insert(0, transform.DOMoveZ(transform.position.z + length * multiplier, (upTime + downTime) * multiplier * heightRatio).SetEase(Ease.Linear));
        _jumpSequence.AppendCallback(EndJump);
    }
    
    private void EndJump()
    {
        _jumpLength = _defaultJumpSpeed;

        _jumpModifier = 1;

        _block = null;

        _isCheckingGround = true;
    }
}
using UnityEngine;
using DG.Tweening;

public class Bird : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _target;    
    [SerializeField] private float _duration;
    [SerializeField] private Vector3 _rotation;

    private float _rotationDuration = 0.1f;

    private int _flyLeftHash = Animator.StringToHash("FlyLeft"); 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            _animator.SetBool(_flyLeftHash, true);

            transform.DOMove(_target.position, _duration);

            transform.DORotate(_rotation, _rotationDuration);
        }
    }
}
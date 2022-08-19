using UnityEngine;

public class BirdTargetMover : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Vector3 _startPosition;
    private float _zOffset = 10;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, _startPosition.z + _target.position.z + _zOffset);
    }
}
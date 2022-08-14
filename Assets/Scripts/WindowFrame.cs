using UnityEngine;

public class WindowFrame : MonoBehaviour
{
    [SerializeField] private Rigidbody _leftPart;
    [SerializeField] private Rigidbody _rightPart;
    [SerializeField] private Rigidbody _upperPart;
    [SerializeField] private Rigidbody _lowerPart;

    private float _force = 3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            DestructPart(_leftPart, Vector3.left);

            DestructPart(_rightPart, Vector3.right);

            DestructPart(_upperPart, Vector3.up);

            if (_lowerPart != null)            
                Destroy(_lowerPart.gameObject);            
        }
    }

    private void DestructPart(Rigidbody part, Vector3 direction)
    {
        part.isKinematic = false;
        part.AddForce(direction * _force, ForceMode.VelocityChange);
    }
}
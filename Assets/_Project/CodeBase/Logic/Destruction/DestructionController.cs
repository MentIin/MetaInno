using UnityEngine;

public class DestructionController : MonoBehaviour
{
    CharacterController _cc;
    [SerializeField] GameObject _explosionParticles;
    [SerializeField] float _desiredSpeed = 9f;

    void Start()
    {
        _cc = GetComponent<CharacterController>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var destructible = hit.gameObject.GetComponent<SimpleDestructible>();

        if (destructible == null) return;
        if (_cc.velocity.magnitude < _desiredSpeed) return;

        var particle = Instantiate(_explosionParticles);
        particle.transform.position = destructible.transform.position;
        Destroy(particle, 1.2f);
        destructible.Destroy(_cc.velocity);
    }
}

using DG.Tweening;
using UnityEngine;

public class SimpleDestructible : MonoBehaviour
{
    public void Destroy(Vector3 velocity)
    {
        // Flatten the velocity
        velocity = new Vector3(velocity.x, 0f, velocity.z);

        velocity = Vector3.ClampMagnitude(velocity, 16f);

        GetComponent<Collider>().enabled = false;

        // Calculate end position
        Vector3 endPosition = transform.position + velocity;
        
        // Create the arching movement
        Vector3[] path = new Vector3[3];
        path[0] = transform.position; // Start point
        path[1] = transform.position + (velocity * 0.5f) + Vector3.up * 1f; // Arch peak
        path[2] = endPosition; // End point
        
        // Sequence for combined animation
        Sequence throwSequence = DOTween.Sequence();
        
        // Movement along the arch (quadratic Bezier curve)
        throwSequence.Append(transform.DOPath(path, 0.5f, PathType.CatmullRom)
            .SetEase(Ease.OutQuad));
        
        // Scaling down to zero
        throwSequence.Join(transform.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InBack));
        
        // Deactivate when complete
        throwSequence.OnComplete(() => gameObject.SetActive(false));
        
        // Play the sequence
        throwSequence.Play();
    }
}

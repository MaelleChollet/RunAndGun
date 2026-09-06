using UnityEngine;

public class CounterFlip : MonoBehaviour
{
    private Transform parentTransform;

    private void Start()
    {
        parentTransform = transform.parent;
    }

    private void LateUpdate()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(parentTransform.localScale.x) * Mathf.Sign(parentTransform.localScale.x);
        // Simplification : on annule l'effet du flip parent
        scale.x = 1f / Mathf.Sign(parentTransform.localScale.x);
        transform.localScale = scale;
    }
}
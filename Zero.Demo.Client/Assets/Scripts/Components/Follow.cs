using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Mask = Vector3.one;
    public Vector3 Offset;
    public bool Lerp;
    public float LerpScalar = 10;

    private void LateUpdate()
    {
        if (Target == null)
        {
            return;
        }

        var position = Target.position;
        var targetPosition = new Vector3(position.x * Mask.x, position.y * Mask.y, position.z * Mask.z) + Offset;
        transform.position = Lerp ? Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * LerpScalar) : targetPosition;
    }
}
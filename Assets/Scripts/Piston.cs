using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Piston : MonoBehaviour
{
    public float speed = 2f;
    [Tooltip("Total distance the piston will move from its start position.")]
    public float distance = 3f;

    public enum MoveDirection
    {
        PosX, NegX, PosY, NegY, PosZ, NegZ,    // world cube faces
        LocalPosX, LocalNegX, LocalPosY, LocalNegY, LocalPosZ, LocalNegZ, // local axes
        TransformForward, TransformBack, TransformUp, TransformDown, TransformRight, TransformLeft, // transform.* shortcuts
        Custom
    }
    public MoveDirection direction = MoveDirection.PosX;
    [Tooltip("Used when Direction is Custom (local or world depending on UseLocalForCustom).")]
    public Vector3 customDirection = Vector3.right;
    [Tooltip("If true, Custom direction is interpreted in local space.")]
    public bool useLocalForCustom = false;

    private Vector3 startPosition;
    private Vector3 moveVector;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Recompute moveVector each frame so local directions follow rotation
        moveVector = GetDirectionVector(direction);
        float offset = Mathf.PingPong(Time.time * speed, distance);
        transform.position = startPosition + moveVector.normalized * offset;
    }

    private Vector3 GetDirectionVector(MoveDirection dir)
    {
        switch (dir)
        {
            // World axes (cube faces)
            case MoveDirection.PosX: return Vector3.right;
            case MoveDirection.NegX: return Vector3.left;
            case MoveDirection.PosY: return Vector3.up;
            case MoveDirection.NegY: return Vector3.down;
            case MoveDirection.PosZ: return Vector3.forward;
            case MoveDirection.NegZ: return Vector3.back;

            // Local axes via transform.TransformDirection
            case MoveDirection.LocalPosX: return transform.TransformDirection(Vector3.right);
            case MoveDirection.LocalNegX: return transform.TransformDirection(Vector3.left);
            case MoveDirection.LocalPosY: return transform.TransformDirection(Vector3.up);
            case MoveDirection.LocalNegY: return transform.TransformDirection(Vector3.down);
            case MoveDirection.LocalPosZ: return transform.TransformDirection(Vector3.forward);
            case MoveDirection.LocalNegZ: return transform.TransformDirection(Vector3.back);

            // Direct transform shortcuts (same as local but clearer intent)
            case MoveDirection.TransformForward: return transform.forward;
            case MoveDirection.TransformBack: return -transform.forward;
            case MoveDirection.TransformUp: return transform.up;
            case MoveDirection.TransformDown: return -transform.up;
            case MoveDirection.TransformRight: return transform.right;
            case MoveDirection.TransformLeft: return -transform.right;

            // Custom
            case MoveDirection.Custom:
                return useLocalForCustom ? transform.TransformDirection(customDirection) : customDirection;

            default:
                return Vector3.right;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 dir = GetDirectionVector(direction);
        Vector3 start = Application.isPlaying ? startPosition : transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(start, start + dir.normalized * distance);
        Gizmos.DrawSphere(start + dir.normalized * distance, 0.1f);
    }
#endif
}
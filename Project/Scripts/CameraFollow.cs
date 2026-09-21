using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject currentTarget;

    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smoothTime = 0.3f;

    private Vector3 currentVelocity = Vector3.zero;

    void Update()
    {
        if (currentTarget == null) return;

        Vector3 targetPosition = currentTarget.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}
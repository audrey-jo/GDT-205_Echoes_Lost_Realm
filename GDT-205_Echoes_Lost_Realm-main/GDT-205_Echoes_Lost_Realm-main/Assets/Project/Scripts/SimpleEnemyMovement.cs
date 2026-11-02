using UnityEngine;

public class SimpleEnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    [Header("Movement Type")]
    [Header("0: Circle, 1: Horizontal, 2: Vertical")]
    [SerializeField] int movementType = 0;

    public float distance = 5f;

    [Header("Direction")]
    public float direction = 1f;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        switch (movementType)
        {
            case 0:
                CircleMovement();
                break;
            case 1:
                HorizontalMovement();
                break;
            case 2:
                VerticalMovement();
                break;
        }
    }

    public void CircleMovement()
    {
        Vector3 moveDirection = new(Mathf.Sin(Time.time), Mathf.Cos(Time.time), 0);
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);
    }

    public void HorizontalMovement()
    {
        float baseZero = initialPosition.x + Mathf.PingPong(Time.time * moveSpeed, distance) * direction;
        transform.position = new Vector3(baseZero, transform.position.y, transform.position.z);
    }

    public void VerticalMovement()
    {
        float baseZero = initialPosition.y + Mathf.PingPong(Time.time * moveSpeed, distance) * direction;
        transform.position = new Vector3(transform.position.x, baseZero, transform.position.z);
    }
}
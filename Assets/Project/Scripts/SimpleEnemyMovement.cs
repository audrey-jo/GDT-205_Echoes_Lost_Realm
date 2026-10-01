using UnityEngine;
using UnityEngine.Events;

public class SimpleEnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    [Header("Movement Type")]
    [Header("0: Circle, 1: Horizontal, 2: Vertical")]
    [SerializeField] int movementType = 0;

    UnityEvent circleMovement;
    UnityEvent horizontalMovement;
    UnityEvent verticalMovement;

    public float xDistance = 5f;
    public float yDistance = 5f;

    private Vector3 initialPosition;


    private void Start()
    {
        circleMovement = new UnityEvent();
        horizontalMovement = new UnityEvent();
        verticalMovement = new UnityEvent();
        circleMovement.AddListener(CircleMovement);
        horizontalMovement.AddListener(HorizontalMovement);
        verticalMovement.AddListener(VerticalMovement);

        initialPosition = transform.position;
    }

    private void Update()
    {
        switch (movementType)
        {
            case 0:
                circleMovement.Invoke();
                break;
            case 1:
                horizontalMovement.Invoke();
                break;
            case 2:
                verticalMovement.Invoke();
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
        float baseZero = initialPosition.x + Mathf.PingPong(Time.time * moveSpeed, xDistance);
        transform.position = new Vector3(baseZero, transform.position.y, transform.position.z);
    }

    public void VerticalMovement()
    {
        float baseZero = initialPosition.y + Mathf.PingPong(Time.time * moveSpeed, yDistance);
        transform.position = new Vector3(transform.position.x, baseZero, transform.position.z);
    }
}
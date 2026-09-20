using UnityEngine;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    [Header("Movement Type")]
    [Header("0: Circle, 1: Horizontal, 2: Vertical")]
    [SerializeField] int movementType = 0;

    UnityEvent circleMovement;
    UnityEvent horizontalMovement;
    UnityEvent verticalMovement;

    private void Start()
    {
        circleMovement = new UnityEvent();
        horizontalMovement = new UnityEvent();
        verticalMovement = new UnityEvent();
        circleMovement.AddListener(CircleMovement);
        horizontalMovement.AddListener(HorizontalMovement);
        verticalMovement.AddListener(VerticalMovement);
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
        Vector3 moveDirection = new Vector3(Mathf.Sin(Time.time), Mathf.Cos(Time.time), 0);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }

    public void HorizontalMovement()
    {
        float moveX = Mathf.PingPong(Time.time * moveSpeed, 10) - 5;
        transform.position = new Vector3(moveX, transform.position.y, transform.position.z);
    }

    public void VerticalMovement()
    {
        float moveY = Mathf.PingPong(Time.time * moveSpeed, 10) - 5;
        transform.position = new Vector3(transform.position.x, moveY, transform.position.z);
    }
}
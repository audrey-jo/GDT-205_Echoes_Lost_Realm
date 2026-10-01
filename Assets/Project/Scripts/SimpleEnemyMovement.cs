using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SimpleEnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    [Header("Movement Type")]
    [Header("0: Circle, 1: Horizontal, 2: Vertical")]
    [SerializeField] int movementType = 0;

    UnityEvent circleMovement;
    UnityEvent horizontalMovement;
    UnityEvent verticalMovement;

    private float startXPos;
    private float startYPos;

    private float sec;

    private void Start()
    {
        circleMovement = new UnityEvent();
        horizontalMovement = new UnityEvent();
        verticalMovement = new UnityEvent();
        circleMovement.AddListener(CircleMovement);
        horizontalMovement.AddListener(HorizontalMovement);
        verticalMovement.AddListener(VerticalMovement);

        startXPos = transform.position.x;
        startYPos = transform.position.y;
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

        sec += Time.deltaTime;
    }

    public void CircleMovement()
    {
        Vector3 moveDirection = new Vector3(Mathf.Sin(Time.time), Mathf.Cos(Time.time), 0);
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);
    }

    public void HorizontalMovement()
    {
        Vector3 moveDirection = Vector3.left;
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);
        if (sec >= 2f)
        {
            moveDirection = Vector3.right;
            transform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);
            if (sec >= 4f)
            {
                sec = 0f;
            }
        }
    }

    public void VerticalMovement()
    {
        Vector3 moveDirection = Vector3.up;
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);
        if (sec >= 2f)
        {
            moveDirection = Vector3.down;
            transform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);
            if (sec >= 4f)
            {
                sec = 0f;
            }
        }
    }
}
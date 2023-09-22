using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody rb;

    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    void MovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");

        float z = Input.GetAxisRaw("Vertical");

        direction = new Vector3(x, 0, z).normalized;
    }

    void Move()
    {
        rb.velocity = new Vector3(direction.x * moveSpeed, 0, direction.z * moveSpeed);
    }
}
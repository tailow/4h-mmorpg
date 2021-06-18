using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float sensitivity;
    public float jumpForce;

    Camera playerCamera;

    Rigidbody rigid;

    private void Start()
    {
        playerCamera = Camera.main;
        rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // movement
        Vector3 movementDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 movement = movementDir * movementSpeed * Time.deltaTime;
        rigid.MovePosition(rigid.position + transform.TransformDirection(movement));
    }

    private void Update()
    {
        // camera rotation
        Vector3 cameraDir = new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0);
        Vector3 rotation = cameraDir * sensitivity * Time.deltaTime;
        transform.Rotate(new Vector3(0, rotation.x, 0));

        playerCamera.transform.RotateAround(rigid.position, transform.right, -rotation.y);

        if (Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector3.up * jumpForce * 100, ForceMode.Impulse);
        }
    }
}

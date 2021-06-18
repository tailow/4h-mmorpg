using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float sensitivity;
    public float jumpForce;
    public float defaultCameraDistance;

    public Animator animator;

    public AudioClip jumpSound;

    public AudioSource source;

    float cameraDistance = 10000;

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

        animator.SetFloat("Speed", movementSpeed * movement.magnitude);

        animator.SetFloat("Direction", Input.GetAxisRaw("Horizontal"));
    }

    private void Update()
    {
        animator.SetBool("Jump", false);

        // camera clipping
        Vector3 cameraDefaultPos = transform.position + (playerCamera.transform.position - transform.position).normalized * defaultCameraDistance;

        RaycastHit hit;

        if (Physics.Raycast(transform.position,
                            playerCamera.transform.position - transform.position,
                            out hit,
                            Vector3.Distance(cameraDefaultPos, transform.position)))
        {
            cameraDistance = Vector3.Distance(hit.point, transform.position) - 1;
        }

        else
        {
            cameraDistance = 10000;
        }

        playerCamera.transform.position = transform.position + (playerCamera.transform.position - transform.position).normalized * Mathf.Min(defaultCameraDistance, cameraDistance);

        // camera rotation
        Vector3 cameraDir = new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0);
        Vector3 rotation = cameraDir * sensitivity * Time.deltaTime;
        transform.Rotate(new Vector3(0, rotation.x, 0));

        float xRotation = -rotation.y;

        if (playerCamera.transform.rotation.x > 0.6f) xRotation = Mathf.Clamp(xRotation, xRotation, 0);
        else if (playerCamera.transform.rotation.x < -0.6f) xRotation = Mathf.Clamp(xRotation, 0, xRotation);

        playerCamera.transform.RotateAround(rigid.position, transform.right, xRotation);

        // jumping
        if (Input.GetButtonDown("Jump"))
        {
            if (Physics.Raycast(transform.position, Vector3.down, 1.6f))
            {
                animator.SetBool("Jump", true);

                source.clip = jumpSound;

                if (!source.isPlaying)
                    source.Play();
            }
        }

        // zooming
        defaultCameraDistance = Mathf.Clamp(defaultCameraDistance - Input.mouseScrollDelta.y, 2, 20);
    }
}

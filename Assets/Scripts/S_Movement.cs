using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class S_Movement : MonoBehaviour
{
    public float groundCheckRadius = 0.05f;
    public float crouchingSpeed = 600f;
    public float kickDownForce = 3000f;
    public float kickDownDistance = 100f;
    public float jumpForce;
    public float yAxis;
    public float xAxis;
    public float speed;

    public int maxJumps = 2;
    public int curJumps;

    public bool isKickingDown;
    public bool isReadyToKickDown;
    public bool isGrounded;

    public Vector3 cameraDirFront;
    public Vector3 cameraDirRight;
    public Vector3 cameraRot;
    public Vector3 input;

    public Transform feetPos;
    public GameObject GFX;
    public Camera camera;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        curJumps = maxJumps;
    }

    void Update()
    {
        yAxis = Input.GetAxis("Vertical");
        xAxis = Input.GetAxis("Horizontal");

        GetCameraRotation();
        KickDown();
        Jump();
    }

    private void FixedUpdate()
    {
        if (isKickingDown)
            rb.velocity = new Vector3(0, -kickDownForce, 0);
        else
            rb.velocity = new Vector3(input.x * speed * Time.fixedDeltaTime, rb.velocity.y, input.z * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = collision.gameObject.CompareTag("Ground");
        isKickingDown = false;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = !collision.gameObject.CompareTag("Ground");
    }

    private void GetCameraRotation()
    {
        cameraRot = camera.transform.rotation.eulerAngles;
        cameraDirFront = new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z);
        cameraDirRight = new Vector3(camera.transform.right.x, 0, camera.transform.right.z);
        input = new Vector3(xAxis, 0, yAxis);
        input = cameraDirFront.normalized * input.z + cameraDirRight.normalized * input.x;
    }

    private void Jump()
    {
        if (isGrounded)
            curJumps = maxJumps;
        if (Input.GetKeyDown(KeyCode.Space) && curJumps > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(0, jumpForce, 0);
            isGrounded = false;
            curJumps--;
        }
    }

    private void KickDown()
    {
        isReadyToKickDown = Physics.Raycast(feetPos.position, -feetPos.up * kickDownDistance, kickDownDistance);
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (isReadyToKickDown)
                isKickingDown = true;
        }
        if(!isReadyToKickDown)
            isKickingDown = false;
    }
}
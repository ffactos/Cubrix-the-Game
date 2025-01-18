using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System.Collections;

public class S_Movement : MonoBehaviour
{
    [SerializeField]
    float checkRadius = .5f;

    public float groundCheckRadius = 0.05f;
    public float kickDownDistance = 100f;
    public float crouchingSpeed = 600f;
    public float kickDownForce = 3000f;
    public float minJumpHeight;
    public float jumpForce;
    public float yAxis;
    public float xAxis;
    public float speed;

    public int maxJumps = 2;
    public int curJumps;

    public bool isKickingDown;
    public bool isGrounded;
    bool isWallJumping;
    bool onRightSide;
    bool onLeftSide;

    public Vector3 cameraDirFront;
    public Vector3 cameraDirRight;
    public Vector3 cameraRot;
    public Vector3 input;
    public LayerMask ground;

    public Transform feetPos;
    public GameObject GFX;
    public Camera cam;
    public Rigidbody rb;
    public Transform rightHand;
    public Transform leftHand;

    RaycastHit rightHit;
    RaycastHit leftHit;

    public UnityEngine.UI.Slider jumpCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        curJumps = maxJumps;
        jumpCounter.maxValue = maxJumps;
        jumpCounter.value = curJumps;
    }

    void Update()
    {
        yAxis = Input.GetAxis("Vertical");
        xAxis = Input.GetAxis("Horizontal");

        AboveGround();

        
        if (onRightSide | onLeftSide)
        {
            curJumps = maxJumps;
            isGrounded = true;
            jumpCounter.DOValue(curJumps, .5f, false);
        }

        isGrounded = Physics.Raycast(feetPos.position, Vector3.down, .1f, ground);

        GetCameraRotation();
        KickDown();
        Jump();
    }

    private void FixedUpdate()
    {
        onRightSide = Physics.Raycast(rightHand.position, rightHand.right, out rightHit, checkRadius, ground);
        onLeftSide = Physics.Raycast(leftHand.position, -leftHand.right, out leftHit, checkRadius, ground);

        if (onLeftSide && Input.GetKey(KeyCode.A) && AboveGround() && !isWallJumping)
            rb.velocity = Vector3.Cross(leftHit.normal, transform.up) * speed * Time.fixedDeltaTime * yAxis;
        else if (onRightSide && Input.GetKey(KeyCode.D) && AboveGround() && !isWallJumping)
            rb.velocity = -Vector3.Cross(rightHit.normal, transform.up) * speed * Time.fixedDeltaTime * yAxis;
        else if (isWallJumping)
            rb.velocity = leftHit.normal * jumpForce * 15f + new Vector3(0f, jumpForce / 10f, 0);
        else
            rb.velocity = new Vector3(input.x * speed * Time.fixedDeltaTime, rb.velocity.y, input.z * speed * Time.fixedDeltaTime);

        transform.rotation = new Quaternion(transform.rotation.x, cam.transform.rotation.y, transform.rotation.z, transform.rotation.w);


        isKickingDown = Physics.Raycast(feetPos.position, Vector3.down, .01f);
    }

    private void GetCameraRotation()
    {
        cameraRot = cam.transform.rotation.eulerAngles;
        cameraDirFront = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
        cameraDirRight = new Vector3(cam.transform.right.x, 0, cam.transform.right.z);
        input = new Vector3(xAxis, 0, yAxis);
        input = cameraDirFront.normalized * input.z + cameraDirRight.normalized * input.x;
    }

    private void Jump()
    {
        if (curJumps < maxJumps && Physics.Raycast(feetPos.position, Vector3.down, .01f, ground))
        {
            curJumps = maxJumps;
            jumpCounter.DOValue(curJumps, .5f, false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && curJumps > 0)
        {
            if(onLeftSide)
            {
                rb.AddForce(leftHit.normal * jumpForce * 15);
                rb.AddForce(0, jumpForce * 2, 0);
                Debug.Log("Jumped from Left Side");
            }
            else if(onRightSide)
            {
                rb.AddForce(rightHit.normal * jumpForce * 15);
                rb.AddForce(0, jumpForce * 2, 0);
                Debug.Log("Jumped from Right Side");
            }
            else
            {
                if (rb.velocity.y < 0)
                    rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(0, jumpForce, 0);
                isGrounded = false;
                isKickingDown = isGrounded;
                curJumps--;
                jumpCounter.DOValue(curJumps, .5f, false);
            }
        }
    }

    private void KickDown()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isKickingDown)
        {
            rb.AddForce(0, -jumpForce * 4, 0);
            isKickingDown = true;
        }
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(feetPos.position, Vector3.down, minJumpHeight, ground);
    }

    IEnumerator WallJump()
    {
        isWallJumping = true;
        yield return new WaitForSecondsRealtime(3f);
        isWallJumping = false;
    }

}
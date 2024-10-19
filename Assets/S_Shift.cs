using System.Collections;
using UnityEngine;
using Input = UnityEngine.Input;

public class S_Shift : MonoBehaviour
{

    public Camera camera;
    public Rigidbody rb;

    public float shiftCD = 1f;
    public float shiftDuration;
    public float shiftSpeed;
    public float shiftTime;

    public Vector3 cameraDirFront;
    private Vector3 cameraDirRight;
    private Vector3 cameraRot;
    private Vector3 input;

    private float xAxis;
    private float zAxis;

    private float x;
    private float z;

    public int curShifts;
    private int maxShifts = 3;

    public bool isShifting;

    private void Start()
    {
        curShifts = maxShifts;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        GetCameraRotation();

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if(shiftCD > shiftTime)
            shiftTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && curShifts > 0)
        {
            curShifts--;
            shiftTime = 0f;
            xAxis = input.x;
            zAxis = input.z;
            StartCoroutine(Shift());
        }

        if (shiftCD >= shiftTime)
        {
            if(curShifts < maxShifts)
            {
                shiftTime = 0f;
                curShifts++;
            }
            
        }
    }

    private void FixedUpdate()
    {
        if (isShifting)
            rb.velocity = new Vector3(shiftSpeed * Time.deltaTime * Mathf.Round(xAxis), 0, shiftSpeed * Time.deltaTime * Mathf.Round(zAxis));
    }

    IEnumerator Shift()
    {
        isShifting = true;
        yield return new WaitForSecondsRealtime(shiftDuration);
        isShifting = false;
    }

    private void GetCameraRotation()
    {
        cameraRot = camera.transform.rotation.eulerAngles;
        cameraDirFront = new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z);
        cameraDirRight = new Vector3(camera.transform.right.x, 0, camera.transform.right.z);
        input = new Vector3(x, 0, z);
        input = cameraDirFront.normalized * input.z + cameraDirRight.normalized * input.x;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_JumpPad : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<Rigidbody>().AddForce(0, collision.gameObject.GetComponent<S_Movement>().jumpForce * 3, 0);
    }

}

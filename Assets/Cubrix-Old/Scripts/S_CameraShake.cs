using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class S_CameraShake : MonoBehaviour
{

    [SerializeField]
    S_Movement player;

    float lastDutch;
    [SerializeField]
    float changeDutch = .5f;
    [SerializeField]
    float powerDutch = 5f;

    float lastFOV;
    [SerializeField]
    float changeFOV = .5f;

    float zoomOffset = 0f;
    [SerializeField]
    float zoomAccelerationPower = 2.5f;

    CinemachinePOV pov;

    CinemachineVirtualCamera cinemachine;

    private void Start()
    {
        cinemachine = GetComponent<CinemachineVirtualCamera>();
        lastDutch = -player.xAxis * 5f;
        pov = cinemachine.GetCinemachineComponent<CinemachinePOV>();
    }

    private void Update()
    {
        cinemachine.m_Lens.Dutch = Mathf.Lerp(lastDutch, -player.xAxis * powerDutch, changeDutch);
        lastDutch = Mathf.Lerp(lastDutch, -player.xAxis * powerDutch, changeDutch);

        if (Input.mouseScrollDelta.y > 0 && zoomOffset < 30f)
        {
            zoomOffset += 5f;
        }
        if (Input.mouseScrollDelta.y < 0 && zoomOffset > -10f)
        {
            zoomOffset -= 5f;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            cinemachine.m_Lens.FieldOfView = Mathf.Lerp(lastFOV, 50f, changeFOV);
            lastFOV = Mathf.Lerp(lastFOV, 50f - zoomOffset, changeFOV);
            if(zoomAccelerationPower != 0)
            {
                pov.m_VerticalAxis.m_AccelTime = zoomOffset / zoomAccelerationPower;
                pov.m_VerticalAxis.m_DecelTime = zoomOffset / zoomAccelerationPower;
                pov.m_HorizontalAxis.m_AccelTime = zoomOffset / zoomAccelerationPower;
                pov.m_HorizontalAxis.m_DecelTime = zoomOffset / zoomAccelerationPower;
            }
        }

        else
        {
            cinemachine.m_Lens.FieldOfView = Mathf.Lerp(lastFOV, 90 + player.rb.linearVelocity.magnitude, changeFOV);
            lastFOV = Mathf.Lerp(lastFOV, 90 + player.rb.linearVelocity.magnitude, changeFOV);
            zoomOffset = 0f;
            pov.m_VerticalAxis.m_AccelTime = 0f;
            pov.m_VerticalAxis.m_DecelTime = 0f;
            pov.m_HorizontalAxis.m_AccelTime = 0f;
            pov.m_HorizontalAxis.m_DecelTime = 0f;

        }
    }
}

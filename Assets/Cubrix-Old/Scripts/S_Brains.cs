using DG.Tweening;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Brains : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    Interactable interactable;

    public GameObject inventory;

    [SerializeField]
    float interactDistance;

    private void Update()
    {
        Ray hand = new Ray(cam.transform.position, cam.transform.forward);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!inventory.activeInHierarchy)
                inventory.SetActive(true);
            else
                inventory.SetActive(false);
        }

        if(Physics.Raycast(hand, out RaycastHit hitInfo, interactDistance))
        {
            if (hitInfo.collider.gameObject.GetComponent<Interactable>() != null)
            {
                if(hitInfo.collider.gameObject.GetComponent<Interactable>() != interactable)
                {
                    if (interactable != null)
                        interactable.DisableOutline();
                    interactable = hitInfo.collider.gameObject.GetComponent<Interactable>();
                    interactable.EnableOutline();
                }
            }
            else
            {
                if(interactable != null) 
                    interactable.DisableOutline();
                interactable = null;
            }
        }
        else
        {
            if (interactable != null)
                interactable.DisableOutline();
            interactable = null;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(interactable != null)
            {
                interactable.Interact();
            }
        }
    }   
}

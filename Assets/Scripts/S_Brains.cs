using DG.Tweening;
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

    public UnityEngine.UI.Slider healthCounter;
    public UnityEngine.UI.Slider background;

    public float maxHealth = 100f;
    public float health;

    private void Start()
    {
        health = maxHealth;
        healthCounter.maxValue = maxHealth;
        healthCounter.value = health;
        background.maxValue = maxHealth;
        background.value = health;
    }

    public void GetHit(float dmg)
    {
        if (0 > health - dmg)
        {
            Kill();
            health = 0;
            healthCounter.DOValue(health, 1f, false).SetEase(Ease.OutExpo);
        }
        else
        {
            health -= dmg;
            healthCounter.DOValue(health, 1f, false).SetEase(Ease.OutExpo);
        }

        healthCounter.gameObject.GetComponent<RectTransform>().DOComplete();
        healthCounter.gameObject.GetComponent<RectTransform>().DOShakePosition(.5f, 15f, 1000, 180, false, true, ShakeRandomnessMode.Harmonic);

        Debug.Log("Got hit");

    }

    public void Heal(float heal)
    {
        if (maxHealth < heal + health)
        {
            if(heal + health > background.value)
            {
                background.value = health;
                background.DOValue(maxHealth, 1f, false).SetEase(Ease.OutExpo);
            }
            health = maxHealth;
            healthCounter.DOValue(health, 1f, false).SetEase(Ease.OutExpo);
        }
        else
        {
            if (heal + health > background.value)
            {
                background.value = health;
                background.DOValue(health + heal, 1f, false).SetEase(Ease.OutExpo);
            }
            health += heal;
            healthCounter.DOValue(health, 1f, false).SetEase(Ease.OutExpo);
        }
    }

    public void Kill()
    {
        Debug.Log("Dead");
    }

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

        if (Input.GetKeyDown(KeyCode.T))
        {
            GetHit(30f);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(30f);
        }

        background.value = Mathf.Lerp(background.value, health, .005f);
    }   
}

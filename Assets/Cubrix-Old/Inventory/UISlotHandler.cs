using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class UISlotHandler : MonoBehaviour, IPointerClickHandler
{

    public Item item;
    public Image icon;
    public TextMeshProUGUI itemCountText;
    public InventoryManager inventoryManager;

    public void SetValue(int value)
    {
        item.value = value;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item == null) { return; }

            MouseManager.instance.PickupFromStack(this);
            return;
        }


        MouseManager.instance.UpdateHeldItem(this);
    }

    private void Start()
    {
        if(item != null)
        {
            item = item.Clone();
            icon.sprite = item.icon;
            itemCountText.text = item.value.ToString();
        }
        else
        {
            icon.gameObject.SetActive(false);
            itemCountText.text = string.Empty;
        }

        inventoryManager = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();

    }

    private void Update()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    Debug.Log("Mouse Over: " + EventSystem.current.currentSelectedGameObject.name);
        //}
    }
}

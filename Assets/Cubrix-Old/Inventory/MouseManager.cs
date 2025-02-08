using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static MouseManager instance;
    public Item currentlyHeld;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateHeldItem(UISlotHandler currentSlot)
    {
        Item currentActiveItem = currentSlot.item;

        if ((currentlyHeld != null 
            && currentActiveItem != null) && currentSlot.item.itemName == currentlyHeld.itemName)
        {
            currentSlot.inventoryManager.StackInInventory(currentSlot, currentlyHeld);
            return;
        }

        if (currentSlot.item != null)
            currentSlot.inventoryManager.ClearItemSlot(currentSlot);
        if (currentlyHeld != null)
            currentSlot.inventoryManager.PlaceInInventory(currentSlot, currentlyHeld);

        currentlyHeld = currentActiveItem;

    }

    public void PickupFromStack(UISlotHandler currentSlot)
    {
        if(currentlyHeld != null && currentlyHeld.itemName != currentSlot.item.itemName)
        {
            return;
        } 
        if(currentlyHeld == null)
        {
            currentlyHeld = currentSlot.item.Clone();
            currentlyHeld.value = 0;
        }

        currentlyHeld.value++;
        currentSlot.item.value--;
        currentSlot.itemCountText.text = currentSlot.item.value.ToString();

        if (currentSlot.item.value <= 0)
            currentSlot.inventoryManager.ClearItemSlot(currentSlot);    
    }
}

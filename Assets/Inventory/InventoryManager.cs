using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public List<UISlotHandler> slots = new List<UISlotHandler>();
    [SerializeField]
    GameObject slotPrefab;
    [SerializeField]
    GameObject Content;

    //                                            51      |       50
    public void StackInInventory(UISlotHandler currentSlot, Item item)
    {
        if(currentSlot.item.stackable && currentSlot.item.value + item.value <= item.stack)
        {
            currentSlot.item.value += item.value;
            currentSlot.itemCountText.text = currentSlot.item.value.ToString();
            item = null;
        }
        else if(currentSlot.item.value <= item.value)
        {
            item.value = currentSlot.item.value + item.value - currentSlot.item.stack;
            currentSlot.item.value = currentSlot.item.stack;
            currentSlot.itemCountText.text = currentSlot.item.value.ToString();
        }
        else
        {
            int space = currentSlot.item.value;
            currentSlot.item.value = item.value;
            currentSlot.itemCountText.text = currentSlot.item.value.ToString();
            item.value = space;
        }
    }
    public void PlaceInInventory(UISlotHandler currentSlot, Item item)
    {
        currentSlot.item = item;
        currentSlot.icon.sprite = item.icon;
        currentSlot.itemCountText.text = item.value.ToString();
        currentSlot.icon.gameObject.SetActive(true);
    }

    public void AddInInventory(Item item)
    {
        if (slots.Count != 0)
        {
            bool isAdded = false;

            foreach (var slot in slots)
            {
                if(slot.item.stackable && slot.item.value + item.value <= item.stack && slot.item.itemName == item.itemName)
                {
                    slot.item = slot.item.Clone();  
                    slot.item.value += item.value;
                    slot.itemCountText.text = slot.item.value.ToString();
                    Debug.Log("1");
                    isAdded = true;
                    break;
                }
            }
            if (!isAdded)
            {
                GameObject addedGameObject = Instantiate(slotPrefab, Content.transform);
                UISlotHandler addedSlot = addedGameObject.GetComponent<UISlotHandler>();
                addedSlot.item = item.Clone();
                addedSlot.item = item;
                addedSlot.icon.sprite = item.icon;
                addedSlot.itemCountText.text = item.value.ToString();
                addedSlot.icon.gameObject.SetActive(true);
                slots.Add(addedSlot);
                Debug.Log("2");
            }
        }
        else
        {
            GameObject addedGameObject = Instantiate(slotPrefab, Content.transform);
            UISlotHandler addedSlot = addedGameObject.GetComponent<UISlotHandler>();
            addedSlot.item = item;
            addedSlot.icon.sprite = item.icon;
            addedSlot.itemCountText.text = item.value.ToString();
            addedSlot.icon.gameObject.SetActive(true);
            slots.Add(addedSlot);
        }
    }

    public void ClearItemSlot(UISlotHandler currentSlot)
    {
        currentSlot.item = null;
        currentSlot.icon.sprite = null;
        currentSlot.itemCountText.text = string.Empty;
        currentSlot.icon.gameObject.SetActive(false);
    }
}

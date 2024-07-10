using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using StarterAssets;
using Unity.VisualScripting;
using Unity.Netcode;

public class Inventory : NetworkBehaviour
{
    [Header("UI")]
    public GameObject inventory;
    private List <Slot> allInventorySlots = new List <Slot> ();
    public List <Slot> inventorySlots = new List <Slot> ();
    public List <Slot> hotbarSlots = new List <Slot> ();
    public Image crosshair;
    public TMP_Text itemHoverText;

    
    public static Inventory Instance { get; set; }

    [Header("Raycast")]
    public float raycastDistance = 5f;
    public LayerMask itemLayer;
    public Transform dropLocation;// the location items will be dropped from

    [Header("Drag and Drop")]
    public Image dragIconImage;
    private Item currentDraggedItem;
    private int currentDragSlotIndex = -1;

    [Header("Equippable Items")]
    public List<GameObject> equippableItems= new List<GameObject> ();
    public Transform selectedItemýmage;
    public int curHotbarIndex=-1;

    [Header("Crafting")]
    public List<Recipe> itemRecipes=new List<Recipe>();

    public bool isOpen;



    public int hotbarIndex;

    public void Start()
    {
       toggleInventory(false);

        allInventorySlots.AddRange(hotbarSlots);
        allInventorySlots.AddRange(inventorySlots);

        isOpen = false;
        foreach (Slot uislot in allInventorySlots) {
            uislot.initialiseSlot();
        }
    }

    public void Update ()
    {

        itemRaycast(Input.GetKeyDown(KeyCode.E));
        
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            toggleInventory(!inventory.activeInHierarchy);
            
        }
        if (inventory.activeInHierarchy && Input.GetMouseButtonDown(0))
            {

                dragInventoryIcon();
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlayDragSound();
            }
            

        }
        else if(currentDragSlotIndex != -1 && Input.GetMouseButtonUp(0) || currentDragSlotIndex!=-1 && !inventory.activeInHierarchy ) //if hovered over
            {
                dropInventoryIcon();
            }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            dropItem();
        }
        
        
            for (int i = 1; i < hotbarSlots.Count + 1; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                
                    enablehotbarItem(i - 1);
                curHotbarIndex = i - 1;
            }
            }
        if (Input.GetMouseButtonDown(1))
        {
            attemptToUseItem();
        }

        dragIconImage.transform.position=Input.mousePosition;
        
        /*if (Input.GetKeyDown(KeyCode.Tab) && !isOpen)
        {

            Debug.Log("Tab is pressed");
            inventory.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isOpen)
        {
            inventory.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }*/
    }
    private void  itemRaycast(bool hasClicked = false)
    {
        itemHoverText.text = "";
        Ray ray =Camera.main.ScreenPointToRay(crosshair.transform.position);

        RaycastHit hit;
        if (Physics.Raycast(ray,out hit, raycastDistance,itemLayer))
        {
            if (hit.collider !=null)
            {
                if (hasClicked) //pickup
                {
                    Item newItem=hit.collider.GetComponent<Item>();
                    if (newItem)
                    {
                        addItemToInventory(newItem);
                    }
                }
                else //get the name
                {
                    Item newItem = hit.collider.GetComponent<Item>();
                    if (newItem)
                    {
                        itemHoverText.text = newItem.name;
                    }
                }
            }
        }
    }

    public void addItemToInventory(Item itemToAdd)
    {
        int leftoverQuantity = itemToAdd.currentQuantity;
        Slot openSlot = null;
        for (int i = 0; i < allInventorySlots.Count; i++)
        {
            SoundManager.instance.PlayPickupSound();

            Item heldItem = allInventorySlots[i].getItem();
            if (heldItem != null && itemToAdd.name == heldItem.name) {// this is the stack functions 
                int freeSpaceInSlot = heldItem.maxquantity - heldItem.currentQuantity;
                if (freeSpaceInSlot>=leftoverQuantity)
                {
                    heldItem.currentQuantity += leftoverQuantity;
                    Destroy(itemToAdd.gameObject);
                    allInventorySlots[i].updateData();
                    return;

                }
                else
                {
                    heldItem.currentQuantity = heldItem.maxquantity;
                    leftoverQuantity -= freeSpaceInSlot;
                }
            }
            else if (heldItem==null)
            {
                if (!openSlot)
                openSlot = allInventorySlots[i];
            }
            allInventorySlots[i].updateData();
        }
        if (leftoverQuantity >0 && openSlot)
        {
            openSlot.setItem(itemToAdd);
            itemToAdd.currentQuantity = leftoverQuantity;
            itemToAdd.gameObject.SetActive(false);
        }
        else
        {
            itemToAdd.currentQuantity = leftoverQuantity;
        }
    }
    public void toggleInventory(bool enable)
    {

        inventory.SetActive(enable);
        SoundManager.instance.PlayInventoryOpenSound();
        Cursor.lockState = enable ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = enable;

        if (!enable)
        {
            foreach(Slot curSlot in allInventorySlots)
                curSlot.hovered = false;
        }
        //Disable camera rotation
        /*Camera.main.GetComponent<FirsPersonCamera>().sensitivity = enable ? 0 : 2;*/ //fix this !!!!!!!!!!!!!!!!!
    }

    private void dropItem()
    {
        for (int i = 0; i< allInventorySlots.Count; i++) 
        {
            Slot curSlot= allInventorySlots[i];
            if (curSlot.hovered && curSlot.hasItem())
            {
            /*    curSlot.getItem().gameObject.SetActive(true);
                curSlot.getItem().transform.position = dropLocation.position;
                curSlot.setItem(null);
                break;*/

                Item droppedItem=Instantiate(curSlot.getItem().gameObject,
                    dropLocation.position,
                    Quaternion.identity).GetComponent<Item>();
                droppedItem.gameObject.SetActive(true);
                droppedItem.currentQuantity = 1;

                curSlot.getItem().currentQuantity -= 1;
               // SoundManager.instance.PlayDropSound();
                curSlot.updateData();

                if (curSlot.getItem().currentQuantity==0)
                    curSlot.setItem(null);

                break;
            }
        }
    }

    public void dragInventoryIcon()
    {

        for (int i = 0; i < allInventorySlots.Count; i++)
        {
            Slot curSlot = allInventorySlots[i];
            if (curSlot.hovered && curSlot.hasItem())
            {

                currentDragSlotIndex = i; // updates the current drag slot index variable

                currentDraggedItem=curSlot.getItem();
                dragIconImage.sprite = currentDraggedItem.icon;
                dragIconImage.color = new Color(1, 1, 1, 1); // make the follow mouse icon opaque (visible)

                curSlot.setItem(null); //remove the item from the slot we just picked up the item from
            }
        }
    } 
    public void dropInventoryIcon()
    {
        dragIconImage.sprite = null;
        dragIconImage.color = new Color(1,1,1,0);

        for (int i = 0; i < allInventorySlots.Count; i++)
        {
            Slot curSlot = allInventorySlots[i];
            if (curSlot.hovered)
            {
                if (curSlot.hasItem()) //swapping the hovered top the top items
                {

                    Item itemToSwap = curSlot.getItem();

                    curSlot.setItem(currentDraggedItem);

                    allInventorySlots[currentDragSlotIndex].setItem(itemToSwap);

                    resetDragVariables();
                    return;
                }
                else// place the item with no swap
                {
                    curSlot.setItem(currentDraggedItem);
                    resetDragVariables();
                    return;
                }
            }
        }
        // if we get to this point we dropped the item in an invalid loction (or closed the inventory)
        allInventorySlots[currentDragSlotIndex].setItem(currentDraggedItem);
        resetDragVariables() ;
    }
    private void resetDragVariables()
    {
        currentDraggedItem = null;
        currentDragSlotIndex = -1;
    }
    public void enablehotbarItem(int hotbarIndex)
    {

        foreach (GameObject a in equippableItems)
        {
            a.SetActive(false);

        }
        Slot hotbarSlot = hotbarSlots[hotbarIndex];
        selectedItemýmage.transform.position = hotbarSlots[hotbarIndex].transform.position;

        if (hotbarSlot.hasItem())
        {
            if (hotbarSlot.getItem().equippableItemIndex != -1)
            {
                
                equippableItems[hotbarSlot.getItem().equippableItemIndex].SetActive(true);

            }
        }
       
        //AxeAnimation.Instance.ResetToolPositionAndRotation();

    }
    public void craftItem(string itemName)
    {
        foreach(Recipe recipe in itemRecipes)
        {
            if (recipe.createdItemPrefab.GetComponent<Item>().name== itemName)
            {
                bool haveAllIngredients = true;
                for (int i = 0; i < recipe.requiredIngredients.Count; i++)
                {
                    if (haveAllIngredients)
                        haveAllIngredients = haveIngredient(recipe.requiredIngredients[i].itemName, recipe.requiredIngredients[i].requiredQuantity);

                }
                if (haveAllIngredients)
                {
                    for(int i = 0; i < recipe.requiredIngredients.Count; i++)
                    {
                        removeIngredient(recipe.requiredIngredients[i].itemName, recipe.requiredIngredients[i].requiredQuantity);
                    }
                    GameObject craftedItem = Instantiate(recipe.createdItemPrefab, dropLocation.position, Quaternion.identity);
                    craftedItem.GetComponent<Item>().currentQuantity = recipe.quantityProduced;

                    addItemToInventory(craftedItem.GetComponent<Item>());
                }
                break;
            }
        }
    }
    private void removeIngredient(string itemName, int quantity)
    {
        if (!haveIngredient(itemName,quantity))
            return;

            int remainingQuantity = quantity;

            foreach ( Slot curSlot in allInventorySlots)
            {
                Item item= curSlot.getItem();

                if (item != null && item.name== itemName)
                {
                    if (item.currentQuantity >= remainingQuantity)
                    {
                    item.currentQuantity -= remainingQuantity;

                    if (item.currentQuantity == 0) { 
                        curSlot.setItem(null);
                    }

                    return;
                    }
                    else
                    {
                    remainingQuantity-= item.currentQuantity;
                    curSlot.setItem(null);
                    }
                }
            }
        
    }
    private bool haveIngredient(string itemName,int quantity)
    {
        int foundQuantity = 0;
        foreach(Slot curSlot in allInventorySlots) {
            if (curSlot.hasItem() && curSlot.getItem().name== itemName)
            {
                foundQuantity += curSlot.getItem().currentQuantity;

                if (foundQuantity >= quantity)
                    return true;
                
            }
        }
        return false;
    }

    private void attemptToUseItem()
    {
        if (curHotbarIndex==-1)
            return;
        Item curItem= hotbarSlots[curHotbarIndex].getItem();
        
        if (curItem) 
        {
            curItem.UseItem();

            if(curItem.currentQuantity!= 0)
                hotbarSlots[curHotbarIndex].updateData();
            else
                hotbarSlots[curHotbarIndex].setItem(null);

            enablehotbarItem(curHotbarIndex);

        }
    }
    ///
}

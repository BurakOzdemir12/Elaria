using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{

    public new string name = "New Item";
    public string description = "New Description";
    public Sprite icon;
    public int currentQuantity=1;
    public int maxquantity=21;

    public int equippableItemIndex = -1;

    [Header("Item Use")]
    public UnityEvent myEvent;
    public bool removeOneUse;

    public void UseItem()
    {
        if (myEvent.GetPersistentEventCount() > 0)
        {
            myEvent.Invoke();
            if (removeOneUse)
            {
                currentQuantity--;
            }
        }
    }
}

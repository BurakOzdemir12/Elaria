using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public GameObject OakTree;
    public GameObject PickUpText;
    public Collider other;
     
    

    void Start()
    {
        /*OakTree.SetActive(false );*/
        PickUpText.SetActive(false );
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag=="Player")
        {
            PickUpText.SetActive(true);
            /*if (Input.GetKey(KeyCode.E))
            {
                this.gameObject.SetActive(false);
                OakTree.SetActive(true);
                PickUpText.SetActive(false);
            }*/
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        PickUpText.SetActive(false);
    }

}

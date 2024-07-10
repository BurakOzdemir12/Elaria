using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; set; }

    public GameObject InventoryMenu;
    public bool isMenuActivated;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        isMenuActivated = false;
        InventoryMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isMenuActivated)
        {

            Debug.Log("Tab is pressed");
            InventoryMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isMenuActivated = true;


        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isMenuActivated)
        {
            InventoryMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isMenuActivated = false;
        }
    }
}
